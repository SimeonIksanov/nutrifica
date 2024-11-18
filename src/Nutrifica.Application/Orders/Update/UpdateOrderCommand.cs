using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Orders.Update;

public record UpdateOrderCommand : ICommand<OrderDto>
{
    public OrderId Id { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public ICollection<UserId> ManagerIds { get; set; } = null!;
}