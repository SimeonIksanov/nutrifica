using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class LastName : ValueObject
{
    private LastName(string value) => Value = value;
    public static LastName Create(string value) => new LastName(value);
    public string Value { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    #region Workaroud for filtering with 'searchTerm'

    public static explicit operator string(LastName lastName) => lastName.Value;

    #endregion
}