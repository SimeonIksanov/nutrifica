using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;

public sealed class ProductId : ValueObject
{
    public static ProductId Create(int value) => new ProductId(value);
    private ProductId(int value) => Value = value;
    public int Value { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
