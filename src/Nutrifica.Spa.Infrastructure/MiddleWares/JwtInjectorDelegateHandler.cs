using System.Net.Http.Headers;

using Blazored.LocalStorage;

using Nutrifica.Api.Contracts.Authentication;

namespace Nutrifica.Spa.Infrastructure.MiddleWares;

public class JwtInjectorDelegateHandler(ILocalStorageService storage) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var requestUri = request.RequestUri?.AbsolutePath.TrimEnd('/') ?? string.Empty;
        bool isAuthRequest = requestUri.EndsWith(Routes.AuthenticationEndpoints.Login)
                             || requestUri.EndsWith(Routes.AuthenticationEndpoints.RefreshToken);
        if (!isAuthRequest)
        {
            var jwt = await storage.GetItemAsStringAsync(nameof(TokenResponse.Jwt), cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}