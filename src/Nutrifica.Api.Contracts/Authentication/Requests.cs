namespace Nutrifica.Api.Contracts.Authentication;

public record TokenRequest(
    string Username,
    string Password);

public record RefreshTokenRequest(
    string Jwt,
    string RefreshToken);

public record LogoutRequest(
    string Jwt,
    string RefreshToken);