namespace Nutrifica.Api.Contracts.Authentication;

public record TokenResponse(string Jwt, string RefreshToken);
