using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Application.Models.Notifications;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Notifications.Get;

public class GetNotificationsQueryHandler : IQueryHandler<GetNotificationsQuery, ICollection<NotificationDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationsQueryHandler(ICurrentUserService currentUserService,
        INotificationRepository notificationRepository)
    {
        _currentUserService = currentUserService;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<ICollection<NotificationDto>>> Handle(GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = new NotificationFilter(request.Since, request.Till, _currentUserService.UserId);

        var notificationList = await GetNotifications(filter, cancellationToken);
        ICollection<NotificationDto> dto = notificationList
            .Select(x => x.ToDto())
            .ToArray();
        return Result.Success(dto);
    }

    private async Task<ICollection<NotificationModel>> GetNotifications(NotificationFilter filter,
        CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetAsync(filter, cancellationToken);
        return notifications;
    }
}