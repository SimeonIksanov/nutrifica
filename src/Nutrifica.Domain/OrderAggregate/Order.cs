using Nutrifica.Domain.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Common.Interfaces;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.OrderAggregate.Entities;
using Nutrifica.Domain.OrderAggregate.Enums;
using Nutrifica.Domain.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Shared.Enums;
using Nutrifica.Domain.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.OrderAggregate;

public class Order : Entity<OrderId>, IAggregateRoot
{
    private Order()
    {
    }

    public ClientId ClientId { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public State State { get; set; }
    public DateTime CreatedAt { get; init; }
    public ICollection<OrderItem> Items { get; private set; } = null!;
    public decimal TotalSum { get; }
    public HashSet<UserId> Operators { get; private set; } = null!;

    public static Order Create(ClientId clientId, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        ArgumentNullException.ThrowIfNull(userId);

        var order = new Order
        {
            Status = OrderStatus.New,
            ClientId = clientId,
            Operators = new HashSet<UserId> { userId },
            State = State.Active
        };
        return order;
    }
}