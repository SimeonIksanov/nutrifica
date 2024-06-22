using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Services.Storage;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public interface IUserService
{
    Task<IResult<User>> SendAuthenticateRequestAsync(TokenRequest request, CancellationToken ct);
    Task<User?> SendRefreshTokensRequestAsync(CancellationToken ct);
    void ClearBrowserUserData();
}