using System.Net.Http.Json;

using Nutrifica.Api.Contracts.Products;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;

namespace Nutrifica.Spa.Infrastructure.Services.Products;

public class ProductService : ServiceBase, IProductService
{
    public async Task<IResult<PagedList<ProductDto>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        var requestUri = ProductsEndpoints.Get + queryParams;
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<ProductDto>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<PagedList<ProductDto>>(ProductServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult> CreateAsync(ProductCreateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(ProductsEndpoints.Create, dto, cancellationToken);
            return await HandleResponse<int>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<ProductDto>(ProductServiceErrors.FailedToCreate);
        }
    }

    public Task<IResult> UpdateAsync(ProductUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }
}