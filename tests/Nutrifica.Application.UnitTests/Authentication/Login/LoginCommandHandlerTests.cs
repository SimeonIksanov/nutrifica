using Moq;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Authentication.Login;
using Nutrifica.Application.Interfaces.Services;
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
    private readonly LoginCommandHandler _sut = null!;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();

    public LoginCommandHandlerTests()
    {
        _sut = new LoginCommandHandler(
            _jwtFactoryMock.Object,
            _passwordHasherMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async void Handle_When_InvalidUsernameProvided_Should_ReturnErrorUserBadLoginOrPass()
    {
        var command = new LoginCommand { Username = "admin", Password = "admin" };

        // Act
        Result<TokenResponse> result = await _sut.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("User.BadLoginOrPassword", result.Error.Code);
    }

    [Fact]
    public async void Handle_When_InvalidPasswordProvided_Should_ReturnErrorUserBadLoginOrPass()
    {
        // Arrange
        User userInStore = CreateUser();

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userInStore)
            .Verifiable();

        var command = new LoginCommand { Username = "admin", Password = "admin" };

        // Act
        Result<TokenResponse> result = await _sut.Handle(command, default);

        // Assert
        _userRepositoryMock.VerifyAll();
        Assert.True(result.IsFailure);
        Assert.Equal("User.BadLoginOrPassword", result.Error.Code);
    }

    [Fact]
    public async void Handle_When_ValidCredsProvided_Should_SaveRefreshTokenAndReturnTokens()
    {
        // Arrange
        User userInStore = CreateUser();

        _jwtFactoryMock
            .Setup(x => x.GenerateAccessToken(userInStore))
            .Returns("jwt")
            .Verifiable();
        _jwtFactoryMock
            .Setup(x => x.GenerateRefreshToken(It.IsAny<string>()))
            .Returns(new RefreshToken { Token = "token" })
            .Verifiable();

        _passwordHasherMock
            .Setup(x => x.Verify("admin", It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userInStore)
            .Verifiable();
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Verifiable();

        var command = new LoginCommand { Username = "admin", Password = "admin" };

        // Act
        Result<TokenResponse> result = await _sut.Handle(command, default);

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
    public async void Handle_When_UserDisabled_Returns_UserDisabledError()
    {
        // Arrange
        User userInStore = CreateUser();
        userInStore.Enabled = false;

        _passwordHasherMock
            .Setup(x => x.Verify("admin", It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true)
            .Verifiable();

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userInStore)
            .Verifiable();

        var command = new LoginCommand { Username = "admin", Password = "admin" };

        // Act
        Result<TokenResponse> result = await _sut.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(UserErrors.Disabled, result.Error);
        _userRepositoryMock.VerifyAll();
        _passwordHasherMock.VerifyAll();
    }

    private static User CreateUser()
    {
        var user = User.Create(
            "admin",
            FirstName.Create("fn"),
            MiddleName.Create("mn"),
            LastName.Create("ln"),
            null, null, null);
        user.Account.Salt = "value";
        user.Account.PasswordHash = "value";
        return user;
    }
}