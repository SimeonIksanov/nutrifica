using Moq;
using Nutrifica.Application.Authentication.Login;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.UnitTests.Authentication.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IPasswordHasherService> _passwordHasherMock = new Mock<IPasswordHasherService>();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
    private readonly Mock<IJwtFactory> _jwtFactoryMock = new Mock<IJwtFactory>();
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly LoginCommandHandler _sut = null!;

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
        var command = new LoginCommand() { Username = "admin", Password = "admin" };

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("User.BadLoginOrPassword", result.Error.Code);
    }

    [Fact]
    public async void Handle_When_InvalidPasswordProvided_Should_ReturnErrorUserBadLoginOrPass()
    {
        // Arrange
        var userInStore = CreateUser();

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userInStore)
            .Verifiable();

        var command = new LoginCommand() { Username = "admin", Password = "admin" };

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        _userRepositoryMock.VerifyAll();
        Assert.True(result.IsFailure);
        Assert.Equal("User.BadLoginOrPassword", result.Error.Code);
    }

    [Fact]
    public async void Handle_When_ValidCredsProvided_Should_SaveRefreshTokenAndReturnTokens()
    {
        // Arrange
        var userInStore = CreateUser();

        _jwtFactoryMock
            .Setup(x => x.GenerateAccessToken(userInStore))
            .Returns("jwt")
            .Verifiable();
        _jwtFactoryMock
            .Setup(x => x.GenerateRefreshToken(It.IsAny<string>()))
            .Returns(new RefreshToken() { Token = "token" })
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

        var command = new LoginCommand() { Username = "admin", Password = "admin" };

        // Act
        var result = await _sut.Handle(command, default);

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
