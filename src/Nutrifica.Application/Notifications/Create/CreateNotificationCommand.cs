using Nutrifica.Application.Abstractions.Messaging;

namespace Nutrifica.Application.Notifications.Create;

public class CreateNotificationCommand : ICommand<Guid>
{
    public DateTime DateTime { get; set; }
    public string Message { get; set; } = null!;
    public Guid? RecipientId { get; set; }
}