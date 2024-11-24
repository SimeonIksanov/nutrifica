namespace Nutrifica.Spa.Infrastructure.Routes;

public static class NotificationsEndpoint
{
    public const string Create = "api/notifications";
    public static string Get(string since, string till) => $"api/notifications?since={since}&till={till}";
}