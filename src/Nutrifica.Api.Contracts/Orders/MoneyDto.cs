namespace Nutrifica.Api.Contracts.Orders;

public record MoneyDto
{
    public decimal Amount { get; set; }
    public CurrencyDto Currency { get; set; } = null!;
}