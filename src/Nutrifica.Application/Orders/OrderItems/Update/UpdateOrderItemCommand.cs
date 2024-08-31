using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace Nutrifica.Application.Orders.OrderItems.Update;

public record UpdateOrderItemCommand : ICommand
{
    public OrderId OrderId { get; set; }
    public ProductId ProductId { get; set; }
    public int Quantity { get; set; }
}