namespace Nutrifica.Api.Contracts.Orders;

public record MoneyDto
{
    public decimal Amount { get; set; } = 0;
    public CurrencyDto Currency { get; set; } = new();
}