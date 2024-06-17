using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IJwtFactory _jwtFactory;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LogoutCommandHandler(IJwtFactory jwtFactory, IUserRepository userRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _jwtFactory = jwtFactory;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
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
            .GetByIdWithRefreshTokensAsync(userId, cancellationToken);

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
                RevokeDescendantRefreshTokens(refreshToken, user, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");
            }
            return Result.Failure(UserErrors.RefreshTokenNotActive);
        }

        RemoveOldRefreshTokens(user);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
    {
        if (!string.IsNullOrWhiteSpace(refreshToken.ReplacedByToken))
        {
            var childToken = user
                .Account
                .RefreshTokens
                .SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);

            if (childToken is null) return;
            if (!childToken.IsRevoked) RevokeRefreshToken(childToken, ipAddress, reason);
            RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }

    private void RemoveOldRefreshTokens(User user)
    {
        user
            .Account
            .RefreshTokens
            .RemoveAll(rt => rt.IsExpired);
    }

    private void RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null)
    {
        token.ReplacedByToken = replacedByToken;
        token.RevokedByIp = ipAddress;
        token.RevokedAt = _dateTimeProvider.UtcNow;
        token.ReasonRevoked = reason;
    }
}
