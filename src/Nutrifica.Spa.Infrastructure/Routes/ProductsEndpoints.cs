namespace Nutrifica.Spa.Infrastructure.Routes;

public static class ProductsEndpoints
{
    public const string Get = "api/products";
    public const string Create = "api/products";
    public static string Update(int productId) => $"api/products/{productId}";
}