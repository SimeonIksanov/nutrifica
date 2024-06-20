using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
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
    private readonly IRefreshTokenService _refreshTokenService;


    public RefreshTokensCommandHandler(IJwtFactory jwtFactory, IUserRepository userRepository, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork, IRefreshTokenService refreshTokenService)
    {
        _jwtFactory = jwtFactory;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _refreshTokenService = refreshTokenService;
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
            .GetByIdIncludeAccountAsync(userId, cancellationToken);

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
                _refreshTokenService.RevokeDescendantRefreshTokens(refreshToken, user, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.RefreshToken}");
            }
            return Result.Failure<TokenResponse>(UserErrors.RefreshTokenNotActive);
        }

        _refreshTokenService.RemoveOldRefreshTokens(user);

        var newRefreshToken = _jwtFactory.GenerateRefreshToken(request.IpAddress);
        var newJwt = _jwtFactory.GenerateAccessToken(user);
        user.Account.RefreshTokens.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(new TokenResponse(newJwt, newRefreshToken.Token));
    }
}
