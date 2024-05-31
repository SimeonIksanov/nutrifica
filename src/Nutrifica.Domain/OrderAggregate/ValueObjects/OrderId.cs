using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.OrderAggregate.ValueObjects;

public sealed class OrderId : ValueObject
{
    public static OrderId Create(int value) => new OrderId(value);
    public OrderId(int value) => Value = value;
    public int Value { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
