using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Shared;

public class Money : ValueObject
{
    private Money() { }

    public Money(decimal amount, Currency currency)
    {
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
        if (first.Currency != second.Currency)
            throw new InvalidOperationException("Валюта должна быть одинаковой");

        return new Money(first.Amount + second.Amount, first.Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Currency;
        yield return Amount;
    }
}