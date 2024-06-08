using Nutrifica.Domain.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Interfaces;

public interface IAuthenticationService
{
    Task<User?> FindByUsernameAsync(string username);
    Task<User?> FindByIdAsync(Guid userId);
    Task<User?> GetUserIfValidPasswordAsync(string userName, string password);
    Task<Result> UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<User?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
