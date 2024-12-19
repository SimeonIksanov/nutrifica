using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Products;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ProductAggregate;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Infrastructure.Services.SortAndFilter;
using Nutrifica.Shared.Wrappers;

using Sieve.Services;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly ISieveProcessor _sieveProcessor;

    public ProductRepository(AppDbContext context, ISieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<ProductModel?> GetProductModelByIdAsync(ProductId productId, CancellationToken ct)
    {
        var query = from product in _context.Products.AsNoTracking()
            where product.Id.Equals(productId)
            select new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Details = product.Details,
                Price = product.Price,
                State = product.State
            };
        var model = await query.FirstOrDefaultAsync(ct);
        return model;
    }

    public async Task<PagedList<ProductModel>> GetByFilterAsync(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var query = from product in _context.Products.AsNoTracking()
            select new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Details = product.Details,
                Price = product.Price,
                State = product.State
            };
        var pagedList =
            await query.SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }

    public async Task<Product?> GetByIdAsync(ProductId productId, CancellationToken ct)
    {
        var product = await _context
            .Products
            .FirstOrDefaultAsync(x => x.Id.Equals(productId), ct);
        return product;
    }

    public void Add(Product product) => _context.Products.Add(product);
}