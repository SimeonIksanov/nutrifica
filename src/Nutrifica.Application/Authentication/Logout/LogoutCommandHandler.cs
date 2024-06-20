using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IJwtFactory _jwtFactory;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRefreshTokenService _refreshTokenService;

    public LogoutCommandHandler(IJwtFactory jwtFactory, IUserRepository userRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IRefreshTokenService refreshTokenService)
    {
        _jwtFactory = jwtFactory;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _refreshTokenService = refreshTokenService;
    }


    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var userId = await _jwtFactory
            .GetUserIdAsync(request.Jwt);
        if (userId.IsEmpty)
        {
            return Result.Failure(UserErrors.BadJwt);
        }

        var user = await _userRepository
            .GetByIdIncludeAccountAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }

        var refreshToken = user
            .Account
            .RefreshTokens
            .FirstOrDefault(x => x.Token.Equals(request.RefreshToken));

        if (refreshToken is null)
        {
            return Result.Failure(UserErrors.RefreshTokenNotFound);
        }

        if (!refreshToken.IsActive)
        {
            if (refreshToken.IsRevoked)
            {
                //some one already revoked it, looks like tokens were stollen
                _refreshTokenService.RevokeDescendantRefreshTokens(refreshToken, user, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");
            }
            return Result.Failure(UserErrors.RefreshTokenNotActive);
        }

        _refreshTokenService.RevokeRefreshToken(refreshToken, request.IpAddress, "Logout", null);
        _refreshTokenService.RemoveOldRefreshTokens(user);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
