using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.Get;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PagedList<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<PagedList<UserResponse>>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var userPagedList = await _userRepository
            .GetByFilterAsync(request.queryParams, cancellationToken);

        var responseList = PagedList<UserResponse>.Create(
            userPagedList.Items.Select(x => x.ToUserResponse()).ToList(),
            userPagedList.Page,
            userPagedList.PageSize,
            userPagedList.TotalCount);

        return Result.Success(responseList);
    }
}