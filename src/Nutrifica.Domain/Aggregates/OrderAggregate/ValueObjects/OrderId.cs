using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;

public sealed class OrderId : ValueObject
{
    public static OrderId CreateUnique() => Create(Guid.NewGuid());
    public static OrderId Create(Guid value) => new OrderId(value);
    public OrderId(Guid value) => Value = value;
    public Guid Value { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value.ToString();
}
