using System.Net.Http.Json;

using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Routes;

namespace Nutrifica.Spa.Infrastructure.Services.Notifications;

public class NotificationService : ServiceBase, INotificationService
{
    public NotificationService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IResult> CreateAsync(NotificationCreateDto dto, CancellationToken cancellationToken)
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

    public async Task<IResult<ICollection<NotificationDto>>> GetAsync(DateTime since, DateTime till, CancellationToken cancellationToken)
    {
        var requestUri = NotificationsEndpoint.Get(ToUTC(since).AddDays(-1).ToString("s"), ToUTC(till).AddDays(1).ToString("s"));
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
    private DateTime ToUTC(DateTime dateTime) => dateTime.ToUniversalTime();
    private DateTime FromUTC(DateTime dateTime) => dateTime.ToLocalTime();
}