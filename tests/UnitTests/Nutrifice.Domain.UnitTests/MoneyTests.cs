using Nutrifica.Domain.Shared;

namespace Nutrifice.Domain.UnitTests;

public class MoneyTests
{
    [Theory]
    [MemberData(nameof(PositiveSingleCurrency))]
    public void Add_When_Positive_Equal_Currency_Should_Return_Sum(Money first, Money second, Money expected)
    {
        var actual = first + second;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(PositiveAndZeroSingleCurrency))]
    public void Add_When_Positive_And_Zero_Equal_Currency_Should_Return_Sum(Money first, Money second, Money expected)
    {
        var actual = first + second;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Add_When_Zero_And_Zero_Should_Return_Sum()
    {
        var actual = Money.Zero() + Money.Zero();
        Assert.Equal(Money.Zero(), actual);

        actual = Money.Zero(Currency.Rur) + Money.Zero();
        Assert.Equal(Money.Zero(), actual);

        actual = Money.Zero() + Money.Zero(Currency.Rur);
        Assert.Equal(Money.Zero(), actual);
    }

    [Fact]
    public void Add_Different_Currencies_One_is_Zero_Returns_NonZero()
    {
        var first = new Money(2, Currency.Rur);
        var second = Money.Zero(Currency.Eur);
        var sum = first + second;
        Assert.Equal(new Money(2, Currency.Rur), sum);

        sum = second + first;
        Assert.Equal(new Money(2, Currency.Rur), sum);
    }

    [Fact]
    public void Add_Different_Currencies_Both_NonZero_Throws()
    {
        var action = () => new Money(2, Currency.Usd) + new Money(3, Currency.Eur);
        Assert.Throws<InvalidOperationException>(action);
    }

    public static IEnumerable<object[]> PositiveSingleCurrency()
    {
        yield return
        [
            new Money(1, Currency.Rur),
            new Money(2, Currency.Rur),
            new Money(3, Currency.Rur)
        ];
        yield return
        [
            new Money(10, Currency.Rur),
            new Money(2, Currency.Rur),
            new Money(12, Currency.Rur)
        ];
    }

    public static IEnumerable<Object[]> PositiveAndZeroSingleCurrency()
    {
        yield return
        [
            Money.Zero(),
            new Money(2, Currency.Rur),
            new Money(2, Currency.Rur)
        ];
        yield return
        [
            Money.Zero(Currency.Rur),
            new Money(2, Currency.Rur),
            new Money(2, Currency.Rur)
        ];

        yield return
        [
            new Money(2, Currency.Rur),
            Money.Zero(),
            new Money(2, Currency.Rur)
        ];
        yield return
        [
            new Money(2, Currency.Rur),
            Money.Zero(Currency.Rur),
            new Money(2, Currency.Rur)
        ];
    }
}