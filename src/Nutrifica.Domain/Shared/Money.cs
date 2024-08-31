using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class Money : ValueObject
{
    private Money() { }

    public Money(decimal amount, Currency currency)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; private set; }
    public Currency Currency { get; init; }

    public static Money Zero() => new Money(0, Currency.None);
    public static Money Zero(Currency currency) => new Money(0, currency);
    public bool IsZero => this == Zero(Currency);

    public static Money operator +(Money first, Money second)
    {
        if (!first.IsZero && !second.IsZero && first.Currency != second.Currency)
            throw new InvalidOperationException("Валюта должна быть одинаковой");

        var sum = first.IsZero
            ? second
            : second.IsZero
                ? first
                : new Money(first.Amount + second.Amount, Currency.FromCode(first.Currency.Code));
        return sum.IsZero ? Money.Zero() : sum;
    }

    public static Money operator -(Money first, Money second)
    {
        if (first.Currency != second.Currency)
            throw new InvalidOperationException("Валюта должна быть одинаковой");

        var dif = new Money(first.Amount - second.Amount, Currency.FromCode(first.Currency.Code));
        return dif.IsZero ? Money.Zero() : dif;
    }

    public static Money operator *(Money money, int multiplier) =>
        new Money(money.Amount * multiplier, Currency.FromCode(money.Currency.Code));

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Currency;
        yield return Amount;
    }
}