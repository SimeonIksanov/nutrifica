namespace Nutrifica.Api.Contracts.Orders;

public record OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = String.Empty;
    public MoneyDto UnitPrice { get; set; } = null!;
    public int Quantity { get; set; }
}

public record OrderItemCreateDto
{
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public record OrderItemUpdateDto
{
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public record OrderItemRemoveDto
{
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
}
