namespace Nutrifica.Api.Contracts.Authentication;

public class TokenRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public record RefreshTokenRequest(string Jwt, string RefreshToken);
public record LogoutRequest(string RefreshToken);
