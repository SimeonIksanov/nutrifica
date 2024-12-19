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
    // private HashSet<OrderManager> _orderManager = null!;
    private List<OrderItem> _orderItems = null!;

    // ReSharper disable once UnusedMember.Local
    private Order() { }

    public DateTime CreatedOn { get; set; }
    public UserId CreatedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
    public ClientId ClientId { get; private set; } = null!;
    public OrderStatus Status { get; set; }

    public State State { get; set; }

    public Money TotalSum { get; private set; } = Money.Zero();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.ToList();
    // public IReadOnlyCollection<UserId> ManagerIds => _orderManager.Select(om => om.UserId).ToList();

    public static Order Create(ClientId clientId, UserId userId)
    {
        var order = new Order()
        {
            Id = OrderId.CreateUnique(),
            ClientId = clientId,
            // _orderManager = new HashSet<OrderManager>(),
            _orderItems = new List<OrderItem>(),
            Status = OrderStatus.New,
            State = State.Active,
            CreatedBy = userId,
        };
        // order.SetManagerId(userId);
        return order;
    }

    public void AddItem(OrderItem item)
    {
        var currentOrderItem = _orderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (currentOrderItem is not null)
        {
            currentOrderItem.UpdateQuantity(currentOrderItem.Quantity + item.Quantity);
        }
        else
        {
            _orderItems.Add(item);
        }

        TotalSum += item.UnitPrice * item.Quantity;
    }

    public void UpdateItem(OrderItem item)
    {
        var currentOrderItem = _orderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (currentOrderItem is null) return;
        currentOrderItem.UpdateQuantity(item.Quantity);
        currentOrderItem.UnitPrice = item.UnitPrice;

        TotalSum = _orderItems
            .Select(x => x.UnitPrice * x.Quantity)
            .Aggregate((a, b) => a + b);
    }

    public void RemoveItem(OrderItem item)
    {
        var currentOrderItem = _orderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
        if (currentOrderItem is null) return;
        _orderItems.Remove(currentOrderItem);

        TotalSum -= currentOrderItem.UnitPrice * currentOrderItem.Quantity;
    }

    // public void SetManagerIds(ICollection<UserId> managerIds)
    // {
    //     _orderManager.Clear();
    //     foreach (var id in managerIds)
    //         _orderManager.Add(new OrderManager { OrderId = Id, UserId = id });
    // }
    // private void SetManagerId(UserId managerId) => SetManagerIds( [managerId] );
}