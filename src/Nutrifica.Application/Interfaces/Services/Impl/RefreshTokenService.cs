using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;

namespace Nutrifica.Application.Interfaces.Services.Impl;

public class RefreshTokenService(IDateTimeProvider dateTimeProvider) : IRefreshTokenService
{
    public void RemoveOldRefreshTokens(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user
            .Account
            .RefreshTokens
            .RemoveAll(rt => rt.IsExpired);
    }

    public void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
    {
        ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        if (!string.IsNullOrWhiteSpace(refreshToken.ReplacedByToken))
        {
            var childToken = user
                .Account
                .RefreshTokens
                .SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);

            if (childToken is null) return;
            if (!childToken.IsRevoked) RevokeRefreshToken(childToken, ipAddress, reason);
            RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }

    public void RevokeRefreshToken(RefreshToken refreshToken, string ipAddress, string? reason = null, string? replacedByToken = null)
    {
        ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));
        refreshToken.ReplacedByToken = replacedByToken;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.RevokedAt = dateTimeProvider.UtcNow;
        refreshToken.ReasonRevoked = reason;
    }
}
