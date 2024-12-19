using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Models.Products;

public class ProductModel
{
    public ProductId Id { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public Money Price { get; set; } = null!;
    public State State { get; set; }
}