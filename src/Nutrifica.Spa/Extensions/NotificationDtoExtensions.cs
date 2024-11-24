using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Spa.Models.Notifications;

namespace Nutrifica.Spa.Extensions;

public static class NotificationDtoExtensions
{
    public static IList<NotificationItem> ToCalendarItems(this ICollection<NotificationDto> notificationDtos)
    {
        return notificationDtos
            .Select(x => new NotificationItem
            {
                Id = x.Id,
                Start = x.DateTime,
                End = x.DateTime.AddMinutes(15),
                Text = x.Message,
                Sender = x.Sender,
            })
            .ToList();
    }
}