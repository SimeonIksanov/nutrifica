using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.NotificationAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.NotificationAggregate;

public class Notification : Entity<NotificationId>, IAggregateRoot, IAuditableEntity
{
    private Notification() { }

    public static Notification Create(DateTime dateTime, string message, UserId recipient)
    {
        return new Notification()
        {
            Id = NotificationId.CreateUnique(), DateTime = dateTime, Message = message, RecipientId = recipient
        };
    }
    public DateTime DateTime { get; private set; }
    public string Message { get; private set; } = null!;
    public UserId RecipientId { get; private set; } = null!;
    public UserId CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
}