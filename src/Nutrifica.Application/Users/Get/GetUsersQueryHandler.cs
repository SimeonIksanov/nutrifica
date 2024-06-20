using Nutrifica.Api.Contracts.Users;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Application.Models.Users;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.Get;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IPagedList<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IPagedList<UserResponse>>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var userPagedList = await _userRepository
            .GetByFilterAsync(request.QueryParams, cancellationToken);

        var responseList = userPagedList
            .ProjectItems(x => x.ToUserResponse());

        return Result.Success(responseList);
    }
}