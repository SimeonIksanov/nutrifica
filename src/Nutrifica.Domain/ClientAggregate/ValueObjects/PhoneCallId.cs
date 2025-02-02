using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.ClientAggregate.ValueObjects;

public class PhoneCallId : ValueObject
{
    public static PhoneCallId Create(int value) => new PhoneCallId(value);
    private PhoneCallId(int value) => Value = value;
    
    public int Value { get; private set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}