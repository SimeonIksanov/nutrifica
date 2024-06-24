using System.Security.Claims;

using Nutrifica.Shared.Enums;

namespace Nutrifica.Spa.Infrastructure.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ClaimsPrincipal ToClaimsPrincipal() =>
        new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[] {
                    new Claim(ClaimTypes.Sid, Id),
                    new Claim(ClaimTypes.NameIdentifier, Username),
                    new Claim(ClaimTypes.Name, FirstName),
                    new Claim(ClaimTypes.Surname, LastName),
                    new Claim(ClaimTypes.Role, Role)
                },
                "Nutrifica"));

    public static User FromClaimsPrincipal(ClaimsPrincipal principal) =>
        new User()
        {
            Id = principal.FindFirst(ClaimTypes.Sid)?.Value ?? "",
            Username = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
            FirstName = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            // MiddleName = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            LastName = principal.FindFirst(ClaimTypes.Surname)?.Value ?? "",
            Role = StringToEnumString(principal.FindFirst(ClaimTypes.Role)?.Value ?? "") ,
        };

    private static string StringToEnumString(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;

        if (Enum.TryParse(typeof(UserRole), value, true, out var role))
        {
            return role.ToString();
        }

        return string.Empty;
    }
}