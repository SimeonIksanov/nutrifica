using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public interface IAuthenticationService
{
    Task<IResult> LoginAsync(TokenRequest request, CancellationToken ct);
    Task<IResult> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct);
    Task<IResult> LogOutAsync(LogoutRequest request, CancellationToken ct);
}