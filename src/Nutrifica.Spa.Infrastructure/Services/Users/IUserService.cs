using Nutrifica.Api.Contracts.Users.Requests;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Users;

public interface IUserService
{
    Task<IResult<PagedList<UserResponse>>> Get(CancellationToken cancellationToken);
    Task<IResult<UserResponse>> Create(UserCreateRequest request, CancellationToken cancellationToken);
    Task<IResult> ChangePasswordAsync(UserChangePasswordRequest request, CancellationToken cancellationToken);
    Task<IResult> ResetPasswordAsync(UserResetPasswordRequest request, CancellationToken cancellationToken);
}