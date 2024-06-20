using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IUserRepository
{
    void Add(User user);
    Task<User?> GetByIdAsync(UserId userId, CancellationToken ct = default);
    Task<User?> GetByIdWithRefreshTokensAsync(UserId userId, CancellationToken ct = default);
    Task<User?> GetByIdWithPasswordHashAsync(UserId userId, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<IPagedList<UserModel>> GetByFilterAsync(UserQueryParams requestQueryParams, CancellationToken cancellationToken);
    Task<UserModel?> GetDetailedByIdAsync(UserId id, CancellationToken cancellationToken);
}
