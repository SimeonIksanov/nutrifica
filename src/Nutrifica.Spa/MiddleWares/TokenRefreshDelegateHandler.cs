using System.Net;
using System.Net.Http.Json;

using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;
using Nutrifica.Spa.Infrastructure.Services;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.MiddleWares;

public class TokenRefreshDelegateHandler(
    ITokenService tokenService,
    NutrificaAuthenticationStateProvider authenticationStateProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        bool isAuthRequest = IsAuthenticationRequest(GetRequestUri(request));
        bool isJwtExpired = await tokenService.IsAccessTokenExpiredAsync(cancellationToken);
        if (isJwtExpired && !isAuthRequest)
        {
            IResult result = await authenticationStateProvider.RefreshTokensAsync(cancellationToken);
            if (result.IsFailure)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = JsonContent.Create(new ProblemDetails
                    {
                        Type = "RefreshTokensFailure",
                        Status = 400,
                        Detail = result.Error.Description,
                        Title = result.Error.Code
                    })
                };
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