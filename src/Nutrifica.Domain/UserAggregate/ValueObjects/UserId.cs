using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.UserAggregate.ValueObjects;

public sealed class UserId : ValueObject
{
    public static UserId Create(int value) => new UserId(value);
    private UserId(int value) => Value = value;
    public int  Value { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}