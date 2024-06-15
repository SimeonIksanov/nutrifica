using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class Email : ValueObject
{
    public static Email Create(string value) => new Email(value);
    private Email(string value) => Value = value;
    public string Value { get; set; } = string.Empty;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value;
}
