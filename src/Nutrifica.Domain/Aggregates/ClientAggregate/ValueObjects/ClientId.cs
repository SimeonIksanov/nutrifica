using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

public sealed class ClientId : ValueObject
{
    public static ClientId CreateUnique() => Create(Guid.CreateVersion7());
    public static ClientId Create(Guid value) => new(value);
    private ClientId(Guid value) => Value = value;

    public Guid Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value.ToString();
}
