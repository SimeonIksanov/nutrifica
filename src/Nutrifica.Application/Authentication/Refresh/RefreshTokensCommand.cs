using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Abstractions.Messaging;

namespace Nutrifica.Application;

public record RefreshTokensCommand(string Jwt, string RefreshToken, string IpAddress)
    : ICommand<TokenResponse>
{
}
