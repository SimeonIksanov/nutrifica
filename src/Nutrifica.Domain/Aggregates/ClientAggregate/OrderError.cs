using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public static class OrderError
{
    public static Error OrderNotFound = new Error(
        "Order.UserNotFound",
        "Order not found.");
}
public static class ProductError
{
    public static Error ProductNotFound = new Error(
        "Product.ProductNotFound",
        "Product not found.");
}