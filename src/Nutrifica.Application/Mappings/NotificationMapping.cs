using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Application.Models.Notifications;

namespace Nutrifica.Application.Mappings;

public static class NotificationMapping
{
    public static NotificationDto ToDto(this NotificationModel notification)
    {
        return new NotificationDto()
        {
            Id = notification.Id,
            DateTime = notification.DateTime,
            Message = notification.Message,
            CreatedOn = notification.CreatedOn,
            Sender = notification.CreatedBy?.ToUserShortDto(),
            Recipient = notification.Recipient?.ToUserShortDto(),
        };
    }
}