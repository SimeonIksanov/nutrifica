using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

public class Comment : ValueObject
{
    public static Comment Create(string value) => new Comment(value);
    private Comment(string value) => Value = value;
    public string Value { get; } = string.Empty;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
