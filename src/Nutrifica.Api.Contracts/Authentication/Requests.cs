namespace Nutrifica.Api.Contracts.Authentication;

public class TokenRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public record RefreshTokenRequest(string jwt, string refreshToken);
public record LogoutRequest(string refreshToken);