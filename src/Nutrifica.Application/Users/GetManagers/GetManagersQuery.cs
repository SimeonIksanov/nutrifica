using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.GetManagers;

public record GetManagersQuery() : IQuery<ICollection<UserShortDto>>;

public class GetManagersQueryHandler : IQueryHandler<GetManagersQuery, ICollection<UserShortDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetManagersQueryHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ICollection<UserShortDto>>> Handle(GetManagersQuery request,
        CancellationToken cancellationToken)
    {
        var managersList = await _userRepository
            .GetManagers(_currentUserService.UserId, cancellationToken);
        ICollection<UserShortDto> dto = managersList.Select(x => x.ToUserShortDto()).ToList();
        return Result.Success(dto);
    }
}