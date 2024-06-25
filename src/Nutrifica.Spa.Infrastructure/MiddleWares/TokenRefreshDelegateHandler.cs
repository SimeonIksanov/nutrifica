using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Infrastructure.MiddleWares;

public class TokenRefreshDelegateHandler(IAuthenticationService authService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var requestUri = request.RequestUri?.AbsolutePath.TrimEnd('/') ?? string.Empty;
        bool isAuthRequest = requestUri.EndsWith(Routes.AuthenticationEndpoints.Login)
                             || requestUri.EndsWith(Routes.AuthenticationEndpoints.RefreshToken);
        if (!(await authService.IsJwtValidAsync(cancellationToken)) && !isAuthRequest)
        {
            _ = await authService.SendRefreshTokensRequestAsync(cancellationToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}