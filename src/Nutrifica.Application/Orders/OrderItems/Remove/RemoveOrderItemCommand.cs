using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace Nutrifica.Application.Orders.OrderItems.Remove;

public record RemoveOrderItemCommand : ICommand
{
    public OrderId OrderId { get; set; } = null!;
    public ProductId ProductId { get; set; } = null!;
}