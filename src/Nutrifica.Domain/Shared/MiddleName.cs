using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class MiddleName : ValueObject
{
    public static MiddleName Create(string value) => new MiddleName(value);
    private MiddleName(string value) => Value = value;
    public string Value { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    #region Workaroud for filtering with 'searchTerm'

    public static explicit operator string(MiddleName middleName) => middleName.Value;

    #endregion
}