using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

public sealed class UserId : ValueObject
{
    public static UserId CreateUnique() => Create(Guid.CreateVersion7());
    public static UserId Create(Guid value) => new UserId(value);
    public static UserId Empty => new UserId(Guid.Empty);
    private UserId(Guid value) => Value = value;
    public Guid Value { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public bool IsEmpty => Value == Guid.Empty;
    public override string ToString() => Value.ToString();
}
