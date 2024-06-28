namespace Nutrifica.Spa.Infrastructure.Services;

public interface ITokenService
{
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
    Task<string> GetRefreshTokenAsync(CancellationToken cancellationToken);
    Task SaveAccessTokenAsync(string token, CancellationToken cancellationToken);
    Task SaveRefreshTokenAsync(string token, CancellationToken cancellationToken);
    Task ClearAsync(CancellationToken cancellationToken);
    Task<bool> IsAccessTokenExpiredAsync(CancellationToken cancellationToken = default);
}