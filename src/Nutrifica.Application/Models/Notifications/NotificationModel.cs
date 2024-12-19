using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.NotificationAggregate.ValueObjects;

namespace Nutrifica.Application.Models.Notifications;

public class NotificationModel : IEquatable<NotificationModel>
{
    public NotificationId Id { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public string Message { get; set; } = null!;
    public UserShortModel? Recipient { get; set; }
    public UserShortModel CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }

    public bool Equals(NotificationModel? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((NotificationModel)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => Id.GetHashCode();
}