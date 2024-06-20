using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Authentication.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, TokenResponse>
{
    private readonly IJwtFactory _jwtFactory;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IJwtFactory jwtFactory,
        IPasswordHasherService passwordHasher,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _jwtFactory = jwtFactory;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
        {
            return Result.Failure<TokenResponse>(UserErrors.BadLoginOrPassword);
        }

        if (!_passwordHasher.Verify(request.Password, user.Account.PasswordHash, user.Account.Salt))
        {
            return Result.Failure<TokenResponse>(UserErrors.BadLoginOrPassword);
        }

        if (user.Enabled is false)
        {
            return Result.Failure<TokenResponse>(UserErrors.Disabled);
        }

        var jwt = _jwtFactory.GenerateAccessToken(user);
        var refreshToken = _jwtFactory.GenerateRefreshToken(request.IpAddress);
        var response = new TokenResponse(jwt, refreshToken.Token);

        user.Account.RefreshTokens = new List<RefreshToken>() { refreshToken };
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(response);
    }
}
