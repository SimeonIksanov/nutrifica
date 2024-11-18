using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Orders.OrderItems.Update;

public record UpdateOrderItemCommand : ICommand
{
    public OrderId OrderId { get; set; } = null!;
    public ProductId ProductId { get; set; } = null!;
    public int Quantity { get; set; }
    public Money UnitPrice { get; set; } = null!;
}