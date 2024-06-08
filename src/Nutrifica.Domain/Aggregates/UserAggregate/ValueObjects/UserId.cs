using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

public sealed class UserId : ValueObject
{
    public static UserId CreateUnique() => Create(Guid.NewGuid());
    public static UserId Create(Guid value) => new UserId(value);
    private UserId(Guid value) => Value = value;
    public Guid Value { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
