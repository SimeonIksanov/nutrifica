using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Domain.Aggregates.OrderAggregate.Entities;

public class OrderItem : Entity<int>
{
    public static OrderItem Create(string product, int count, Money price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(product);
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        var orderItem = new OrderItem()
        {
            Product = product,
            Price = price,
            Count = count
        };
        return orderItem;
    }
    internal OrderItem() { }

    public string Product { get; init; } = string.Empty;
    public int Count { get; private set; }
    public Money Price { get; private set; }
}
