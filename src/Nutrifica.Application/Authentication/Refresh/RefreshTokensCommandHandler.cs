using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application;

public class RefreshTokensCommandHandler
    : ICommandHandler<RefreshTokensCommand, TokenResponse>
{
    private readonly IJwtFactory _jwtFactory;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensCommandHandler(IJwtFactory jwtFactory, IUserRepository userRepository, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {
        _jwtFactory = jwtFactory;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TokenResponse>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var userId = await _jwtFactory
            .GetUserIdAsync(request.Jwt);
        if (userId.IsEmpty)
        {
            return Result.Failure<TokenResponse>(UserErrors.BadJwt);
        }

        var user = await _userRepository
            .GetByIdWithRefreshTokensAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<TokenResponse>(UserErrors.UserNotFound);
        }

        var refreshToken = user
            .Account
            .RefreshTokens
            .FirstOrDefault(x => x.Token.Equals(request.RefreshToken));

        if (refreshToken is null)
        {
            return Result.Failure<TokenResponse>(UserErrors.RefreshTokenNotFound);
        }

        if (!refreshToken.IsActive)
        {
            if (refreshToken.IsRevoked)
            {
                //some one already revoked it, looks like tokens were stollen
                RevokeDescendantRefreshTokens(refreshToken, user, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");
            }
            return Result.Failure<TokenResponse>(UserErrors.RefreshTokenNotActive);
        }

        RemoveOldRefreshTokens(user);

        var newRefreshToken = _jwtFactory.GenerateRefreshToken(request.IpAddress);
        var newJwt = _jwtFactory.GenerateAccessToken(user);
        user.Account.RefreshTokens.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(new TokenResponse(newJwt, newRefreshToken.Token));
    }

    private void RemoveOldRefreshTokens(User user)
    {
        user
            .Account
            .RefreshTokens
            .RemoveAll(rt => rt.IsExpired);
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

    private void RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null, string? replacedByToken = null)
    {
        token.ReplacedByToken = replacedByToken;
        token.RevokedByIp = ipAddress;
        token.RevokedAt = _dateTimeProvider.UtcNow;
        token.ReasonRevoked = reason;
    }
}
