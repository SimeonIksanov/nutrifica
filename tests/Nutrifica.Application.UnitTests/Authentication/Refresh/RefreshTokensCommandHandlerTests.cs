using Moq;
using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Impl;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.UnitTests.Authentication.Refresh;

public class RefreshTokensCommandHandlerTests
{
    private readonly Mock<IJwtFactory> _jwtFactoryMock = new Mock<IJwtFactory>();
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new Mock<IDateTimeProvider>();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly RefreshTokensCommandHandler _sut;
    private readonly RefreshTokensCommand _command;

    public RefreshTokensCommandHandlerTests()
    {
        _refreshTokenService = new RefreshTokenService(_dateTimeProviderMock.Object);
        _command = new RefreshTokensCommand(
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zaWQiOiIyMzM4ZDNjMS05MWViLTQxYjQtODhmNi01ZGZiNTE3OTc0MzkiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6InVzZXJuYW1lIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImZuIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc3VybmFtZSI6Imxhc3RuYW1lIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMSIsIm5iZiI6MTcxODQ2MDI1MiwiZXhwIjoxNzE4NDYwNTUyLCJpc3MiOiJJc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9.iyrtP_5pFMT82mbeujcQjay8q4J_R5xpnkRh29C_OH4",
            "asdf",
            "1.1.1.1");
        _sut = new RefreshTokensCommandHandler(
            _jwtFactoryMock.Object,
            _userRepositoryMock.Object,
            _dateTimeProviderMock.Object,
            _unitOfWorkMock.Object,
            _refreshTokenService);
    }

    [Fact]
    public async Task Handle_When_ProvidedJwtOfNotExistingUser_Should_ReturnNotFound()
    {
        // Arrange
        _jwtFactoryMock
            .Setup(x => x.GetUserIdAsync(_command.Jwt))
            .ReturnsAsync(UserId.Empty);

        // Act
        var actual = await _sut.Handle(_command, default);

        // Assert
        Assert.False(actual.IsSuccess);
        Assert.Equal(actual.Error, UserErrors.BadJwt);
    }

    [Fact]
    public async Task Handle_When_ProvidedRefreshTokenNotExists_Should_ReturnNotFound()
    {
        // Arrange
        var refreshTokens = new List<RefreshToken> { new RefreshToken() { Token = "aaaabbbbcccc" } };
        var user = CreateUser(refreshTokens);
        SetupMocks(user);

        // Act
        var actual = await _sut.Handle(_command, default);

        // Assert
        Assert.False(actual.IsSuccess);
        Assert.Equal(UserErrors.RefreshTokenNotFound, actual.Error);
    }

    [Fact]
    public async Task Handle_When_ExpiredRefreshTokensProvided_Should_ReturnTokenExpired()
    {
        // Arrange
        var refreshTokens = new List<RefreshToken> { new RefreshToken() {
            Token = _command.RefreshToken,
            Expires = DateTime.UtcNow.AddMinutes(-20)
        } };
        var user = CreateUser(refreshTokens);
        SetupMocks(user);

        // Act
        var actual = await _sut.Handle(_command, default);

        // Assert
        Assert.False(actual.IsSuccess);
        Assert.Equal(UserErrors.RefreshTokenNotActive, actual.Error);
    }

    [Fact]
    public async Task Handle_When_RevokedRefreshTokensProvided_Should_ReturnTokenNotActive()
    {
        // Arrange
        var refreshTokens = new List<RefreshToken> { new RefreshToken() {
            Token = _command.RefreshToken,
            Expires = DateTime.UtcNow.AddMinutes(20),
            RevokedAt = DateTime.UtcNow.AddMinutes(-1),
            RevokedByIp = "2.2.2.2",
            ReasonRevoked = "test reasons",
        } };
        var user = CreateUser(refreshTokens);
        SetupMocks(user);

        // Act
        var actual = await _sut.Handle(_command, default);

        // Assert
        Assert.False(actual.IsSuccess);
        Assert.Equal(UserErrors.RefreshTokenNotActive, actual.Error);
    }

    [Fact]
    public async Task Handle_When_RevokedRefreshTokensProvided_Should_RevokeAllChildTokens()
    {
        var refreshTokens = new List<RefreshToken>{
            new RefreshToken() { Token = _command.RefreshToken, ReplacedByToken = "aaaaaa", RevokedAt= DateTime.UtcNow.AddMinutes(-1),RevokedByIp = "2.2.2.2", ReasonRevoked = "test reasons",},
            new RefreshToken() { Token = "aaaaaa", ReplacedByToken= "bbbbbb", RevokedAt= DateTime.UtcNow.AddMinutes(-1),RevokedByIp = "2.2.2.2", ReasonRevoked = "test reasons"},
            new RefreshToken() { Token = "bbbbbb"}
        };
        var user = CreateUser(refreshTokens);
        SetupMocks(user);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        // Act
        Assert.False(user.Account.RefreshTokens.Single(x => x.Token == "bbbbbb").IsRevoked);
        var actual = await _sut.Handle(_command, default);

        // Assert
        Assert.True(user.Account.RefreshTokens.Single(x => x.Token == "bbbbbb").IsRevoked);
    }

    [Fact]
    public async Task Handle_When_ValidRefreshTokensProvided_Should_ReturnRefreshedTokenPair()
    {
        // Arrange
        var refreshTokens = new List<RefreshToken> { new RefreshToken() {
            Token = "asdf",
            Expires = DateTime.UtcNow.AddMinutes(10),
        } };
        var user = CreateUser(refreshTokens);
        SetupMocks(user);
        _jwtFactoryMock
            .Setup(x => x.GenerateAccessToken(user))
            .Returns("jwt");
        _jwtFactoryMock
            .Setup(x => x.GenerateRefreshToken(_command.IpAddress))
            .Returns(new RefreshToken { Token = "cccc" });

        // Act
        var actual = await _sut.Handle(_command, default);

        // Assert
        Assert.True(actual.IsSuccess);
        Assert.NotEmpty(actual.Value.Jwt);
        Assert.NotEmpty(actual.Value.RefreshToken);
    }

    private static User CreateUser(List<RefreshToken> refreshTokens)
    {
        var user = User.Create(
            "username",
            FirstName.Create("fn"),
            MiddleName.Create("mn"),
            LastName.Create("lastname"),
            PhoneNumber.Create("5678"),
            Email.Create("email"),
            null);
        user.Account.RefreshTokens = refreshTokens;
        return user;
    }

    private void SetupMocks(User user)
    {
        _jwtFactoryMock
            .Setup(x => x.GetUserIdAsync(_command.Jwt))
            .ReturnsAsync(user.Id);
        _userRepositoryMock
            .Setup(x => x.GetByIdWithRefreshTokensAsync(user.Id, default))
            .ReturnsAsync(user);
    }
}
