using Nutrifica.Application.Models.Orders;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IOrderRepository
{
    void Add(Order order);
    Task<Order?> GetByIdAsync(OrderId orderId, UserId managerId, CancellationToken ct = default);
    Task<OrderModel?> GetOrderModelByIdAsync(OrderId orderId, UserId managerId, CancellationToken ct = default);
    Task<PagedList<OrderModel>> GetByFilterAsync(QueryParams queryParams, UserId managerId, CancellationToken cancellationToken);
}
