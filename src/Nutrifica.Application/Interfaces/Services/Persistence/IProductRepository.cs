using Nutrifica.Application.Models.Products;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ProductAggregate;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId productId, CancellationToken ct = default);
    Task<ProductModel?> GetProductModelByIdAsync(ProductId productId, CancellationToken ct = default);
    Task<PagedList<ProductModel>> GetByFilterAsync(QueryParams queryParams, CancellationToken cancellationToken);
    void Add(Product product);
}
