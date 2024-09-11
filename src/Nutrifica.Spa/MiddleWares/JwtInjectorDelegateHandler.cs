using System.Net.Http.Headers;

using Nutrifica.Spa.Infrastructure.Routes;
using Nutrifica.Spa.Infrastructure.Services;

namespace Nutrifica.Spa.MiddleWares;

public class JwtInjectorDelegateHandler(ITokenService tokenService, IConfiguration configuration) : DelegatingHandler
{
    private readonly string _baseApiUrl = configuration.GetSection("backend").GetValue<string>("authority") ?? string.Empty;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string requestPath = request.RequestUri?.AbsolutePath.TrimEnd('/') ?? string.Empty;
        bool isAuthRequest = requestPath.EndsWith(AuthenticationEndpoints.Login)
                             || requestPath.EndsWith(AuthenticationEndpoints.RefreshToken);
        bool isTrustedUrl = _baseApiUrl.Equals(request.RequestUri?.Authority, StringComparison.OrdinalIgnoreCase);
        if (!isAuthRequest && isTrustedUrl)
        {
            string jwt = await tokenService.GetAccessTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}