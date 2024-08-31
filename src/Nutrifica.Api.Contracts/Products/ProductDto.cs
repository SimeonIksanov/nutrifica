using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Products;

public record ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public MoneyDto Price { get; set; } = null!;
    public State State { get; set; }
}