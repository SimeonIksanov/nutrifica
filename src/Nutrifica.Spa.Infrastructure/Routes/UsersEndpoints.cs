namespace Nutrifica.Spa.Infrastructure.Routes;

public static class UsersEndpoints
{
    public const string Get = "api/users";
    public const string GetManagers = "api/users/managers";
    public const string GetSubordinates = "api/users/subordinates";
    public const string Create = "api/users";
    public static string Update(Guid userId) => $"api/users/{userId.ToString()}";
    public static string ChangePassword(Guid userId) => $"api/users/{userId.ToString()}/changePassword";
    public static string ResetPassword(Guid userId) => $"api/users/{userId.ToString()}/resetPassword";
}