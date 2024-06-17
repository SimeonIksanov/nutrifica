using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class MiddleName : ValueObject
{
    public static MiddleName Create(string value) => new MiddleName(value);
    private MiddleName(string value) => Value = value;
    public string Value { get; set; } = string.Empty;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value;
}

