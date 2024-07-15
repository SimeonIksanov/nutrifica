using Nutrifica.Api.Contracts.Users.Requests;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Users;

public interface IUserService
{
    Task<IResult<PagedList<UserResponse>>> Get(QueryParams queryParams, CancellationToken cancellationToken);
    Task<IResult<UserResponse>> CreateAsync(UserCreateRequest request, CancellationToken cancellationToken);
    Task<IResult<UserResponse>> UpdateAsync(UserUpdateRequest request, CancellationToken cancellationToken);
    Task<IResult> ChangePasswordAsync(UserChangePasswordRequest request, CancellationToken cancellationToken);
    Task<IResult> ResetPasswordAsync(UserResetPasswordRequest request, CancellationToken cancellationToken);
}