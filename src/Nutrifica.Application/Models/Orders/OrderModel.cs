using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Models.Orders;

public class OrderModel
{
    public OrderId Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserShortModel CreatedBy { get; set; } = null!;
    public ClientShortModel Client { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public Money TotalSum { get; set; } = null!;
    public ICollection<OrderItemModel> OrderItems { get; set; } = null!;
    public ICollection<UserShortModel> Managers { get; set; } = null!;
}

public class OrderItemModel
{
    public int Id { get; set; }
    public ProductId ProductId { get; set; }
    public string ProductName { get; set; } = String.Empty;
    public string ProductDetails { get; set; } = String.Empty;
    public Money UnitPrice { get; set; } = null!;
    public int Quantity { get; set; }
}