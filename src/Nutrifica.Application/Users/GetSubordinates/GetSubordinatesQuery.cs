using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.GetSubordinates;

public record GetSubordinatesQuery() : IQuery<ICollection<UserShortDto>>;

public class GetSubordinatesQueryHandler : IQueryHandler<GetSubordinatesQuery, ICollection<UserShortDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetSubordinatesQueryHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ICollection<UserShortDto>>> Handle(GetSubordinatesQuery request,
        CancellationToken cancellationToken)
    {
        var subordinatesList = await _userRepository
            .GetSubordinates(_currentUserService.UserId, cancellationToken);
        ICollection<UserShortDto> dto = subordinatesList.Select(x => x.ToUserShortDto()).ToList();
        return Result.Success(dto);
    }
}
