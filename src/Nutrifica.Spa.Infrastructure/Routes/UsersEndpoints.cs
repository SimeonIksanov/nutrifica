namespace Nutrifica.Spa.Infrastructure.Routes;

public static class UsersEndpoints
{
    public const string Get = "api/users";
    public const string Create = "api/users";
    public static string ChangePassword(Guid userId) => $"api/users/{userId.ToString()}/changePassword";
    public static string ResetPassword(Guid userId) => $"api/users/{userId.ToString()}/resetPassword";
}