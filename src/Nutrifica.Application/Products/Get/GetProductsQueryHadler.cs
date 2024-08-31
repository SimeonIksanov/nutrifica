using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Products.Get;

public class GetProductsQueryHadler : IQueryHandler<GetProductsQuery, PagedList<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHadler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    public async Task<Result<PagedList<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pagedList = await _productRepository
            .GetByFilterAsync(request.QueryParams, cancellationToken);
        var list = PagedList<ProductDto>.Create(
            pagedList.Items.Select(x => x.ToProductDto()).ToList(),
            pagedList.Page,
            pagedList.PageSize,
            pagedList.TotalCount);
        return Result.Success(list);
    }
}