using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Application.Abstractions.Messaging;

namespace Nutrifica.Application.Notifications.Get;

public class GetNotificationsQuery : IQuery<ICollection<NotificationDto>>
{
    public DateTime Since { get; set; }
    public DateTime Till { get; set; }
}