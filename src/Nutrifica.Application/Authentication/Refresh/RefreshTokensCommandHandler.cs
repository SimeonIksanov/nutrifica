using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application;

public class RefreshTokensCommandHandler
    : ICommandHandler<RefreshTokensCommand, TokenResponse>
{
    public Task<Result<TokenResponse>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
