namespace Nutrifica.Spa.Infrastructure.Routes;

public static class ClientsEndpoints
{
    public const string Get = "api/clients";
    public static string GetById(Guid id) => $"api/clients/{id.ToString()}";
    public const string Create = "api/clients";
    public static string Update(Guid id) => $"api/clients/{id.ToString()}";
}