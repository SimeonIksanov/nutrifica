using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Aggregates.UserAggregate;

public interface IUserRepository
{
    void Add(User user);
    Task<User?> GetByIdAsync(UserId userId, CancellationToken ct = default);
    Task<User?> GetByIdWithRefreshTokensAsync(UserId userId, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
}
