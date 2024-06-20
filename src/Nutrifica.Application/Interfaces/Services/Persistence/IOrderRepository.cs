using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId orderId, CancellationToken ct = default);
    void Add(Order order);
}
