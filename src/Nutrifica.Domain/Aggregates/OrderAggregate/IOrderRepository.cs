using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;

namespace Nutrifica.Domain.Aggregates.OrderAggregate;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId orderId, CancellationToken ct = default);
    void Add(Order order);
}
