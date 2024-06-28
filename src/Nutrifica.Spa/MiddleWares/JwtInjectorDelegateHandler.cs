using System.Net.Http.Headers;

using Nutrifica.Spa.Infrastructure.Routes;
using Nutrifica.Spa.Infrastructure.Services;

namespace Nutrifica.Spa.MiddleWares;

public class JwtInjectorDelegateHandler(ITokenService tokenService) : DelegatingHandler
{
    // TODO : Параметризовать базовые адреса бекенда
    private readonly string[] baseApiUrls = ["localhost:5203", "localhost"];

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string requestPath = request.RequestUri?.AbsolutePath.TrimEnd('/') ?? string.Empty;
        bool isAuthRequest = requestPath.EndsWith(AuthenticationEndpoints.Login)
                             || requestPath.EndsWith(AuthenticationEndpoints.RefreshToken);
        bool isTrustedUrl = baseApiUrls.Contains(request.RequestUri?.Authority);
        if (!isAuthRequest && isTrustedUrl)
        {
            string jwt = await tokenService.GetAccessTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}