using Nutrifica.Api.Contracts.Users.Responses;

namespace Nutrifica.Api.Contracts.Notifications;

public record NotificationCreateDto
{
    public DateTime DateTime { get; set; }
    public string Message { get; set; } = null!;
    public Guid? RecipientId { get; set; }
}
public record NotificationDto
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Message { get; set; } = null!;
    public UserShortDto? Sender { get; set; } = null!;
    public UserShortDto? Recipient { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
}
