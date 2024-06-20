using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class FirstName : ValueObject
{
    public static FirstName Create(string value) => new FirstName(value);
    private FirstName(string value) => Value = value;
    public string Value { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    #region Workaroud for filtering with 'searchTerm'

    public static explicit operator string(FirstName firstName) => firstName.Value;

    #endregion
}