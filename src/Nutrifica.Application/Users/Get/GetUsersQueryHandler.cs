using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.Get;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PagedList<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<PagedList<UserDto>>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var userPagedList = await _userRepository
            .GetByFilterAsync(request.QueryParams, cancellationToken);

        var responseList = PagedList<UserDto>.Create(
            userPagedList.Items.Select(x => x.ToUserDto()).ToList(),
            userPagedList.Page,
            userPagedList.PageSize,
            userPagedList.TotalCount);

        return Result.Success(responseList);
    }
}