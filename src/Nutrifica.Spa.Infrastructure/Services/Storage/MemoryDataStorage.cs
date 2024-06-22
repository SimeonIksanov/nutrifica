using System.Security.Claims;

using Nutrifica.Shared.Enums;

namespace Nutrifica.Spa.Infrastructure.Services.Storage;

public class MemoryDataStorage : IDataStorage
{
    private readonly Dictionary<string, string> _storage = new();

    public bool HasKey(string key) => _storage.ContainsKey(key);
    public string GetValue(string key) => _storage[key];
    public bool TryGetValue(string key, out string? value) => _storage.TryGetValue(key, out value);
    public void SetValue(string key, string value) => _storage[key]= value;
    public void Clear() => _storage.Clear();
}

public interface IDataStorage
{
    bool HasKey(string key);
    string GetValue(string key);
    bool TryGetValue(string key, out string? value);
    void SetValue(string key, string value);
    void Clear();
}

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