using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class LastName : ValueObject
{
    public string Value { get; set; } = string.Empty;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value;
}

