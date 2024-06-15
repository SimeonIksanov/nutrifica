using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Abstractions.Messaging;

namespace Nutrifica.Application;

public record RefreshTokensCommand(string jwt, string refreshToken, string ipAddress)
    : ICommand<TokenResponse>
{
}
