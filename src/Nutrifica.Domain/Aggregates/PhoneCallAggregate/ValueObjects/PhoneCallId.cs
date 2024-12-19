using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;

public class PhoneCallId : ValueObject
{
    public static PhoneCallId CreateUnique() => Create(Guid.CreateVersion7());
    public static PhoneCallId Create(Guid value) => new(value);
    private PhoneCallId(Guid value) => Value = value;

    public Guid Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value.ToString();
}