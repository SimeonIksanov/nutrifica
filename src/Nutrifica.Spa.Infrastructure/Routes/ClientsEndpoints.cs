using Nutrifica.Api.Contracts.Clients;

namespace Nutrifica.Spa.Infrastructure.Routes;

public static class ClientsEndpoints
{
    public const string Get = "api/clients";
    public static string GetById(Guid id) => $"api/clients/{id.ToString()}";
    public static string GetPhoneCalls(Guid clientId) => $"api/clients/{clientId.ToString()}/phonecalls";
    public const string Create = "api/clients";
    public static string CreatePhoneCall(Guid clientId) => $"api/clients/{clientId.ToString()}/phonecalls";
    public static string Update(Guid id) => $"api/clients/{id.ToString()}";
}