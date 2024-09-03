using Nutrifica.Api.Contracts.Products;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Products;

public interface IProductService
{
    Task<IResult<PagedList<ProductDto>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<IResult> CreateAsync(ProductCreateDto dto, CancellationToken cancellationToken);
    Task<IResult> UpdateAsync(ProductUpdateDto dto, CancellationToken cancellationToken);
}