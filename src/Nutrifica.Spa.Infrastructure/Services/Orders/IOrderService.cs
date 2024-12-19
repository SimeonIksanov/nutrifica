using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Orders;

public interface IOrderService
{
    Task<IResult<PagedList<OrderDto>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken);
    // Task<IResult<PagedList<OrderDto>>> GetByClientIdAsync(Guid clientId, QueryParams queryParams, CancellationToken cancellationToken);
    Task<IResult<OrderDto>> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    Task<IResult> CreateAsync(OrderCreateDto dto, CancellationToken cancellationToken);
    Task<IResult> UpdateAsync(OrderUpdateDto dto, CancellationToken cancellationToken);

    Task<IResult> AddOrderItemAsync(OrderItemCreateDto dto, CancellationToken cancellationToken);
    Task<IResult> UpdateOrderItemAsync(OrderItemUpdateDto dto, CancellationToken cancellationToken);
    Task<IResult> DeleteOrderItemAsync(OrderItemRemoveDto dto, CancellationToken cancellationToken);
}