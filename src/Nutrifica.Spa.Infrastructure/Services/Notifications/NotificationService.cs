using System.Net.Http.Json;

using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Routes;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Infrastructure.Services.Notifications;

public class NotificationService : ServiceBase, INotificationService, IDisposable
{
    private readonly NutrificaAuthenticationStateProvider _authenticationStateProvider;
    private readonly List<NotificationDto> _nextEvents;
    private readonly CancellationTokenSource _cancellationTokenSource = null!;
    private readonly Task _job;
    private readonly Object _syncRoot = new();

    public NotificationService(IHttpClientFactory httpClientFactory,
        NutrificaAuthenticationStateProvider authenticationStateProvider) : base(httpClientFactory)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _cancellationTokenSource = new CancellationTokenSource();
        _nextEvents = new List<NotificationDto>();
        _job = Task.Run(() => FetchNotifications(_cancellationTokenSource.Token));
    }

    public int BadgeCount => _nextEvents.Count;
    public event Action OnChange;

    private async Task FetchNotifications(CancellationToken cancellationToken)
    {
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(60));
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            var result = await GetAsync(DateTime.Now, DateTime.Now.AddHours(1), _cancellationTokenSource.Token);
            if (result.IsFailure) return;

            var notifications = result.Value
                .Where(x =>
                    x.Recipient is not null
                    && x.Recipient.Id == _authenticationStateProvider.CurrentUser.Id)
                .ToList();
            Console.WriteLine($"{notifications.Count} notifications received");
            lock (_syncRoot)
            {
                _nextEvents.Clear();
                _nextEvents.AddRange(notifications);
            }
            OnChange?.Invoke();
        }

        timer.Dispose();
    }

    public async Task<IResult> CreateAsync(
        NotificationCreateDto dto,
        CancellationToken cancellationToken)
    {
        dto.DateTime = ToUTC(dto.DateTime);
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(NotificationsEndpoint.Create, dto, cancellationToken);
            return await HandleResponse(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(NotificationServiceErrors.FailedToCreate);
        }
    }

    public async Task<IResult<ICollection<NotificationDto>>> GetWithAroundAsync(
        DateTime since,
        DateTime till,
        CancellationToken cancellationToken)
    {
        return await GetAsync(since.AddDays(-1), till.AddDays(1), cancellationToken);
    }

    public async Task<IResult<ICollection<NotificationDto>>> GetAsync(
        DateTime since,
        DateTime till,
        CancellationToken cancellationToken)
    {
        var requestUri =
            NotificationsEndpoint.Get(ToUTC(since).ToString("s"), ToUTC(till).ToString("s"));
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            var items = await HandleResponse<ICollection<NotificationDto>>(response, cancellationToken);
            foreach (var t in items.Value)
            {
                t.DateTime = FromUTC(t.DateTime);
            }

            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список уведомлений: {ex.Message}");
            return Result.Failure<ICollection<NotificationDto>>(NotificationServiceErrors.FailedToLoad);
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _job.Dispose();
        _cancellationTokenSource.Dispose();
    }

    private DateTime ToUTC(DateTime dateTime) => dateTime.ToUniversalTime();
    private DateTime FromUTC(DateTime dateTime) => dateTime.ToLocalTime();
}