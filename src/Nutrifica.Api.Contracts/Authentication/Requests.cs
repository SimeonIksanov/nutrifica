namespace Nutrifica.Api.Contracts.Authentication;

public record TokenRequest(string username, string password);
public record RefreshTokenRequest(string jwt, string refreshToken);
public record LogoutRequest(string refreshToken);