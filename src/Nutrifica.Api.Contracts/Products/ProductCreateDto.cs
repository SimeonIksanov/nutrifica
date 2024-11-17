using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Products;

public record ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public MoneyDto Price { get; set; } = new MoneyDto();
}