using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.UserAggregate.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; set; } = string.Empty;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}