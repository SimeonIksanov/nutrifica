using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class Currency : ValueObject
{
    internal static readonly Currency None = new("");
    public static readonly Currency Rur = new("RUR");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");

    private Currency(string code) => Code = code;

    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code.Equals(code)) ?? throw new ApplicationException("Не корректная валюта");
    }

    public static readonly IReadOnlyCollection<Currency> All = new[] { Rur, Usd, Eur };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
