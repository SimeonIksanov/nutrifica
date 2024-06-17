using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

public class Comment : ValueObject
{
    public string Value { get; set; } = string.Empty;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
