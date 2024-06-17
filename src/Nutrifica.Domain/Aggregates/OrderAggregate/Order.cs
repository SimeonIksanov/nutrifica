using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate.Entities;
using Nutrifica.Domain.Aggregates.OrderAggregate.Enums;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.OrderAggregate;

public sealed class Order : Entity<OrderId>, IAggregateRoot
{
    private Order() { }

    public ClientId ClientId { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public State State { get; set; }
    public DateTime CreatedAt { get; init; }
    public ICollection<OrderItem> Items { get; private set; } = null!;
    public Money TotalSum { get; }
    public HashSet<UserId> Operators { get; private set; } = null!;

    public static Order Create(ClientId clientId, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        ArgumentNullException.ThrowIfNull(userId);

        var order = new Order
        {
            Id = OrderId.CreateUnique(),
            Status = OrderStatus.New,
            ClientId = clientId,
            Operators = new HashSet<UserId> { userId },
            State = State.Active
        };
        return order;
    }
}
