using Microsoft.AspNetCore.Components;

using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Routes;
using Nutrifica.Spa.Infrastructure.Services;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.MiddleWares;

public class TokenRefreshDelegateHandler(
    ITokenService tokenService,
    IAuthenticationService authenticationService,
    NavigationManager navigationManager) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        bool isAuthRequest = IsAuthenticationRequest(GetRequestUri(request));
        bool isJwtExpired = await tokenService.IsAccessTokenExpiredAsync(cancellationToken);
        if (isJwtExpired && !isAuthRequest)
        {
            IResult result = await authenticationService.SendRefreshTokensRequestAsync(cancellationToken);
            if (result.IsFailure)
            {
                navigationManager.NavigateTo("/logout", true);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private static string GetRequestUri(HttpRequestMessage request) =>
        request.RequestUri?.AbsolutePath.TrimEnd('/') ?? string.Empty;


    private static bool IsAuthenticationRequest(string requestUri) =>
        requestUri.EndsWith(AuthenticationEndpoints.Login)
        || requestUri.EndsWith(AuthenticationEndpoints.RefreshToken);
}