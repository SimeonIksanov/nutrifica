using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.ClientAggregate.ValueObjects;

public sealed class ClientId : ValueObject
{
    public static ClientId Create(int value)
    {
        return new(value);
    }
    
    private ClientId(int value)
    {
        Value = value;
    }

    public int Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}