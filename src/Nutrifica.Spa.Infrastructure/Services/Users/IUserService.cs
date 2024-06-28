using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Users;

public interface IUserService
{
    Task<IResult<PagedList<UserResponse>>> Get(CancellationToken cancellationToken);
}