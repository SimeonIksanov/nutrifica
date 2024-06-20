using Nutrifica.Domain.Aggregates.ProductAggregate;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId productId, CancellationToken ct = default);
    void Add(Product product);
}
