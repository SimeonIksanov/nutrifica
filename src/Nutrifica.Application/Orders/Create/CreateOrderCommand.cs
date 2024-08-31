using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Orders.Create;

public record CreateOrderCommand : ICommand<Guid>
{
    public ClientId ClientId { get; set; } = null!;
}