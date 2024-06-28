using System.IdentityModel.Tokens.Jwt;

using Blazored.LocalStorage;

namespace Nutrifica.Spa.Infrastructure.Services;

public class TokenService(ILocalStorageService localStorageService) : ITokenService
{
    private const string AccessToken = "AccessToken";
    private const string RefreshToken = "RefreshToken";

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default) =>
        await localStorageService.GetItemAsStringAsync(AccessToken, cancellationToken) ?? string.Empty;

    public async Task<string> GetRefreshTokenAsync(CancellationToken cancellationToken = default) =>
        await localStorageService.GetItemAsStringAsync(RefreshToken, cancellationToken) ?? string.Empty;

    public async Task SaveAccessTokenAsync(string? token, CancellationToken cancellationToken = default) =>
        await localStorageService.SetItemAsStringAsync(AccessToken, token ?? string.Empty, cancellationToken);

    public async Task SaveRefreshTokenAsync(string? token, CancellationToken cancellationToken = default) =>
        await localStorageService.SetItemAsStringAsync(RefreshToken, token ?? string.Empty, cancellationToken);

    public async Task ClearAsync(CancellationToken cancellationToken)
    {
        await localStorageService.RemoveItemAsync(AccessToken, cancellationToken);
        await localStorageService.RemoveItemAsync(RefreshToken, cancellationToken);
    }

    public async Task<bool> IsAccessTokenExpiredAsync(CancellationToken cancellationToken = default)
    {
        var accessToken = await GetAccessTokenAsync(cancellationToken);
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(accessToken))
        {
            JwtSecurityToken? jwtSecurityToken = tokenHandler.ReadJwtToken(accessToken);
            return DateTime.UtcNow < jwtSecurityToken.ValidFrom
                   || jwtSecurityToken.ValidTo < DateTime.UtcNow;
        }

        return true;
    }
}