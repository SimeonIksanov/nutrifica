namespace Nutrifica.Spa.Models.Notifications;

public class NotificationAddModel
{
    public DateTime? Day { get; set; }
    public TimeSpan? Time { get; set; }
    public string Message { get; set; } = null!;
    public Guid? RecipientId { get; set; } = null!;
}