using System.Dynamic;

using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.NotificationAggregate.ValueObjects;

public sealed class NotificationId : ValueObject
{
    public static NotificationId CreateUnique() => Create(Guid.CreateVersion7());
    public static NotificationId Create(Guid value) => new NotificationId(value);
    public NotificationId(Guid value) => Value = value;
    public Guid Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value.ToString();
}