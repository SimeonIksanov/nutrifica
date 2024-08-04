namespace Nutrifica.Spa.Infrastructure.Routes;

public static class PageUrls
{
    public const string ChangePassword = "/account/changePassword";
    public const string Login = "/login";
    public const string Logout = "/logout";
    public const string Users = "/users";
    public const string Clients = "/clients";
    public static string ClientDetails(Guid clientId) => string.Format($"{PageUrls.Clients}/{clientId}");
}