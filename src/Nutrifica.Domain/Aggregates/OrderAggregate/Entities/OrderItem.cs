using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.OrderAggregate.Entities;

public class OrderItem : Entity<int>
{
    public static OrderItem Create(ProductId productId, int count, decimal price)
    {
        ArgumentNullException.ThrowIfNull(productId);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfNegative(price);

        var orderItem = new OrderItem()
        {
            ProductId = productId,
            Price = price,
            Count = count
        };
        return orderItem;
    }
    internal OrderItem() { }

    public ProductId ProductId { get; init; } = null!;
    public int Count { get; private set; }
    public decimal Price { get; private set; }
}
