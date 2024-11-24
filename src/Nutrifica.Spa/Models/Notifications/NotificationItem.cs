using Heron.MudCalendar;

using Nutrifica.Api.Contracts.Users.Responses;

namespace Nutrifica.Spa.Models.Notifications;

public class NotificationItem : CalendarItem
{
    public new Guid Id { get; set; }
    public UserShortDto? Sender { get; set; } = null;
}