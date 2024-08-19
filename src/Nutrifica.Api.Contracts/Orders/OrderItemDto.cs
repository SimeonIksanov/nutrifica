namespace Nutrifica.Api.Contracts.Orders;

public record OrderItemDto
{
    public MoneyDto Price { get; set; } = null!;
    public int Count { get; set; }
    public string Product { get; set; } = String.Empty;
}