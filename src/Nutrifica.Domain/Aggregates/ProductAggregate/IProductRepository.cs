using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace Nutrifica.Domain.Aggregates.ProductAggregate;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId productId, CancellationToken ct = default);
    void Add(Product product);
}
