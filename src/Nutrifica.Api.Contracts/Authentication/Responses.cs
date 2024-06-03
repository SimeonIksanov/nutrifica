namespace Nutrifica.Api.Contracts.Authentication;

public record TokenResponse(string jwt, string refreshToken);