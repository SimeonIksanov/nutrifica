using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Products.Update;

public record ProductUpdateCommand : ICommand<ProductDto>
{
    public ProductId Id { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public Money Price { get; set; } = null!;
    public State State { get; set; }
}