using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;

namespace Nutrifica.Application.Interfaces.Services;

public interface IRefreshTokenService
{
    void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason);
    void RemoveOldRefreshTokens(User user);
    void RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null);
}
