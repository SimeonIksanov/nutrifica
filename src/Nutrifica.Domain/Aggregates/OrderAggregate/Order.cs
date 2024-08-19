using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate.Entities;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.OrderAggregate;

public sealed class Order : Entity<OrderId>, IAggregateRoot, IAuditableEntity
{
    private readonly HashSet<UserId> _managerIds;
    private readonly List<OrderItem> _orderItems;
    private Order() { }

    private Order(UserId createdBy)
    {
        _managerIds = new HashSet<UserId> { createdBy };
        Status = OrderStatus.New;
        State = State.Active;
    }

    public DateTime CreatedOn { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public UserId LastModifiedBy { get; set; }
    public ClientId ClientId { get; private set; } = null!;
    public OrderStatus Status { get; private set; }

    public State State { get; set; }

    public Money TotalSum { get; } = Money.Zero();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.ToList();
    public IReadOnlyCollection<UserId> ManagerIds => _managerIds.ToList();

    public static Order Create(ClientId clientId, UserId userId)
    {
        var order = new Order(userId) { Id = OrderId.CreateUnique(), ClientId = clientId };
        return order;
    }
}