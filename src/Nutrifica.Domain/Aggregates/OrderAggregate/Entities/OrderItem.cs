using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Domain.Aggregates.OrderAggregate.Entities;

public class OrderItem : Entity<int>
{
    public static OrderItem Create(ProductId productId, int quantity, Money unitPrice)
    {
        ArgumentNullException.ThrowIfNull(productId);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);

        var orderItem = new OrderItem()
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
        return orderItem;
    }
    internal OrderItem() { }

    public ProductId ProductId { get; init; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public void UpdateQuantity(int newQuantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(newQuantity);
        Quantity = newQuantity;
    }
}
