using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Aggregates.UserAggregate;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId userId, CancellationToken ct = default);
    void Add(User user);
}
