using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.ClientAggregate.ValueObjects;

public class PhoneNumber : ValueObject
{
    public static PhoneNumber Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, "phoneNumber");
        return new PhoneNumber(value);
    }
    
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }
        
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}