namespace Nutrifica.Spa.Infrastructure.Routes;

public static class OrdersEndpoints
{
    public const string Get = "api/orders";
    public const string Create = "api/orders";
    public static string Update(int orderId) => $"api/orders/{orderId}";
}