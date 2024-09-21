using Nutrifica.Api.Contracts.Users.Requests;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Users;

public interface IUserService
{
    Task<IResult<PagedList<UserDto>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<IResult<UserDto>> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken);
    Task<IResult<UserDto>> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken);
    Task<IResult> ChangePasswordAsync(UserChangePasswordDto dto, CancellationToken cancellationToken);
    Task<IResult> ResetPasswordAsync(UserResetPasswordDto dto, CancellationToken cancellationToken);
    Task<IResult<ICollection<UserShortDto>>> GetManagersAsync(CancellationToken cancellationToken);
}