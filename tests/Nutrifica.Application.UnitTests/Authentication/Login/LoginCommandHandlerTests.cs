using Moq;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Authentication.Login;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.UnitTests.Authentication.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IJwtFactory> _jwtFactoryMock = new();
    private readonly Mock<IPasswordHasherService> _passwordHasherMock = new();
    private readonly LoginCommandHandler _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly LoginCommand _command;

    public LoginCommandHandlerTests()
    {
        _sut = new LoginCommandHandler(
            _jwtFactoryMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
        _command = new() { Username = "admin", Password = "admin" };
    }

    [Fact]
    public async Task Handle_When_InvalidUsernameProvided_Should_ReturnErrorUserBadLoginOrPass()
    {
        // Act
        Result<TokenResponse> result = await _sut.Handle(_command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("User.BadLoginOrPassword", result.Error.Code);
    }

    [Fact]
    public async Task Handle_When_InvalidPasswordProvided_Should_ReturnErrorUserBadLoginOrPass()
    {
        // Arrange
        User userInStore = CreateUser();

        SetupUserRepositoryMock(userInStore);

        // Act
        Result<TokenResponse> result = await _sut.Handle(_command, default);

        // Assert
        _userRepositoryMock.VerifyAll();
        Assert.True(result.IsFailure);
        Assert.Equal("User.BadLoginOrPassword", result.Error.Code);
    }

    [Fact]
    public async Task Handle_When_ValidCredsProvided_Should_SaveRefreshTokenAndReturnTokens()
    {
        // Arrange
        User userInStore = CreateUser();

        SetupJwtFactoryMock(userInStore);
        SetupPasswordHasherMock();
        SetupUserRepositoryMock(userInStore);
        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        Result<TokenResponse> result = await _sut.Handle(_command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("jwt", result.Value.Jwt);
        Assert.Equal("token", result.Value.RefreshToken);
        Assert.NotEmpty(result.Value.RefreshToken);
        _userRepositoryMock.VerifyAll();
        _jwtFactoryMock.VerifyAll();
        _passwordHasherMock.VerifyAll();
        _unitOfWorkMock.VerifyAll();
    }

    [Fact]
    public async Task Handle_When_UserDisabled_Returns_UserDisabledError()
    {
        // Arrange
        User userInStore = CreateUser();
        // userInStore.Enabled = false;
        userInStore.Disable("test disable");

        SetupPasswordHasherMock();
        SetupUserRepositoryMock(userInStore);

        // Act
        Result<TokenResponse> result = await _sut.Handle(_command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(UserErrors.Disabled, result.Error);
        _userRepositoryMock.VerifyAll();
        _passwordHasherMock.VerifyAll();
    }

    private void SetupPasswordHasherMock()
    {
        _passwordHasherMock
            .Setup(x => x.Verify("admin", It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true)
            .Verifiable();
    }

    private void SetupUserRepositoryMock(User userInStore)
    {
        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(userInStore.Account.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userInStore)
            .Verifiable();
    }

    private void SetupJwtFactoryMock(User userInStore)
    {
        _jwtFactoryMock
            .Setup(x => x.GenerateAccessToken(userInStore))
            .Returns("jwt")
            .Verifiable();

        _jwtFactoryMock
            .Setup(x => x.GenerateRefreshToken(It.IsAny<string>()))
            .Returns(new RefreshToken { Token = "token" })
            .Verifiable();
    }

    private static User CreateUser()
    {
        var user = User.Create(
            "admin",
            FirstName.Create("fn"),
            MiddleName.Create("mn"),
            LastName.Create("ln"),
            null!, null!, null);
        user.Account.Salt = "value";
        user.Account.PasswordHash = "value";
        return user;
    }
}