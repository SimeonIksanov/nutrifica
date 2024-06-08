using MediatR;
using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.CommandAndQueries.Authentication.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    private readonly IJwtFactory _jwtFactory;
    private readonly IPasswordHasherService _passwordHasher;

    public LoginCommandHandler(IJwtFactory jwtFactory, IPasswordHasherService passwordHasher)
    {
        _jwtFactory = jwtFactory;
        _passwordHasher = passwordHasher;
    }

    public Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var salt= new byte[16];
        var pass = "admin";
        var user = CreateUser(pass, salt);
        if (!_passwordHasher.Verify(request.Password, user.Account.PasswordHash, user.Account.Salt))
            return Task.FromResult(Result<TokenResponse>.Fail("bad password"));

        var response = new TokenResponse(
            _jwtFactory.GenerateAccessToken(user), _jwtFactory.GenerateRefreshToken("").Token);
        return Task.FromResult(Result<TokenResponse>.Ok(response));
    }

    private User CreateUser(string pass, byte[]salt)
    {
        var user = User.Create(username: "admin",
            firstName: "f",
            middleName: "m",
            lastname: "l",
            phoneNumber: null,
            email: pass,
            supervisorId: null);
        
        var ps = _passwordHasher.HashPassword("admin",salt);
        
        user.Account.PasswordHash = ps;
        user.Account.Salt = Convert.ToBase64String(salt);
        
        return user;
    }
}