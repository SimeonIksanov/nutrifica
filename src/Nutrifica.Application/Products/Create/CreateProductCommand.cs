using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Products.Create;

public record CreateProductCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public Money Price { get; set; } = null!;
}