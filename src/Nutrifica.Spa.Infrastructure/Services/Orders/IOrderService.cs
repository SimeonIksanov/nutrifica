using System.Net.Http.Json;

using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;

namespace Nutrifica.Spa.Infrastructure.Services.Orders;

public interface IOrderService
{
    Task<IResult<PagedList<OrderDto>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<IResult> CreateAsync(OrderCreateDto dto, CancellationToken cancellationToken);
    Task<IResult> UpdateAsync(OrderUpdateDto dto, CancellationToken cancellationToken);
}

public class OrderService : ServiceBase, IOrderService
{
    public OrderService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IResult<PagedList<OrderDto>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken)
    {
        var requestUri = OrdersEndpoints.Get + queryParams;
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<OrderDto>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<PagedList<OrderDto>>(OrderServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult> CreateAsync(OrderCreateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(OrdersEndpoints.Create, dto, cancellationToken);
            return await HandleResponse<Guid>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(OrderServiceErrors.FailedToCreate);
        }
    }

    public Task<IResult> UpdateAsync(OrderUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}