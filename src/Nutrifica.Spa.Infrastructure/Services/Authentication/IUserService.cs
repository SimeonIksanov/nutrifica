using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public interface IUserService
{
    Task<IResult<User>> SendAuthenticateRequestAsync(TokenRequest request, CancellationToken ct);
    Task<IResult<User>> TryRefreshTokensRequestAsync(CancellationToken ct);
    Task ClearBrowserUserData();
    Task<User?> FetchUserFromBrowser();
    Task SendLogoutRequest(CancellationToken ct);
}