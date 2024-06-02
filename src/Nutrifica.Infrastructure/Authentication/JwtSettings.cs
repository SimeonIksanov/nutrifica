namespace Nutrifica.Infrastructure.Authentication;

public class JwtSettings
{
    public string Audience { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int AccessTokenExpiryMinutes { get; init; }
    public int RefreshTokenExpiryMinutes { get; init; }
}
