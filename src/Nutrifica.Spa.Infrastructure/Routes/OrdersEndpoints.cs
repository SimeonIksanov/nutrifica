namespace Nutrifica.Spa.Infrastructure.Routes;

public static class OrdersEndpoints
{
    public const string Get = "api/orders";
    public const string Create = "api/orders";
    public static string GetById(Guid orderId) => $"api/orders/{orderId.ToString()}";
    public static string Update(Guid orderId) => $"api/orders/{orderId.ToString()}";

    public static string AddItem(Guid orderId) => $"api/orders/{orderId.ToString()}/items";
    public static string UpdateItem(Guid orderId) => $"api/orders/{orderId.ToString()}/items";
    public static string RemoveItem(Guid orderId, int productId) => $"api/orders/{orderId.ToString()}/items/{productId.ToString()}";
}