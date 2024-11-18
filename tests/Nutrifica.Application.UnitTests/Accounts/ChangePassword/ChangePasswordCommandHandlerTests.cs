using Moq;

using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.UnitTests.Utils;
using Nutrifica.Application.Users.ChangePassword;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Application.UnitTests.Accounts.ChangePassword;

public class ChangePasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasherService> _passwordHasherServiceMock;

    private readonly ChangePasswordCommandHandler _sut;

    public ChangePasswordCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _passwordHasherServiceMock = new Mock<IPasswordHasherService>();

        _sut = new ChangePasswordCommandHandler(_userRepositoryMock.Object, unitOfWorkMock.Object,
            _passwordHasherServiceMock.Object);
    }

    [Fact]
    public async Task Handle_When_OwnPassChanged_Returns_Success()
    {
        var user = UserUtils.CreateUser();
        var command = new ChangePasswordCommand(user.Id, "cur", "new");

        SetupRepoMock(user);
        _passwordHasherServiceMock
            .Setup(x => x.Verify(command.CurrentPassword, user.Account.PasswordHash, user.Account.Salt))
            .Returns(true);
        _passwordHasherServiceMock
            .Setup(x => x.HashPassword(command.NewPassword))
            .Returns(("newHash", "newSalt"));

        var actual = await _sut.Handle(command, default);

        Assert.True(actual.IsSuccess);
        Assert.Equal("newHash", user.Account.PasswordHash);
        Assert.Equal("newSalt", user.Account.Salt);
    }

    [Fact]
    public async Task Handle_When_EmptyPass_Returns_WrongPass()
    {
        var user = UserUtils.CreateUser();
        var command = new ChangePasswordCommand(user.Id, string.Empty, "new");
        SetupRepoMock(user);

        var actual = await _sut.Handle(command, default);

        Assert.True(actual.IsFailure);
        Assert.Equal(UserErrors.WrongPassword, actual.Error);
        _passwordHasherServiceMock.Verify(x => x.HashPassword(command.NewPassword), Times.Never);
    }

    [Fact]
    public async Task Handle_When_UserNotFound_Returns_UserNotFound()
    {
        var command = new ChangePasswordCommand(UserId.CreateUnique(), "", "new");
        _userRepositoryMock
            .Setup(x => x.GetByIdIncludeAccountAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        var actual = await _sut.Handle(command, default);

        Assert.True(actual.IsFailure);
        Assert.Equal(UserErrors.UserNotFound, actual.Error);
        _passwordHasherServiceMock.Verify(x => x.HashPassword(command.NewPassword), Times.Never);
    }

    [Fact]
    public async Task Handle_When_WrongCurPass_Returns_WrongPassword()
    {
        var user = UserUtils.CreateUser();
        var command = new ChangePasswordCommand(user.Id, "wrong", "new");
        SetupRepoMock(user);
        _passwordHasherServiceMock
            .Setup(x => x.Verify(command.CurrentPassword, user.Account.PasswordHash, user.Account.Salt))
            .Returns(false);

        var actual = await _sut.Handle(command, default);

        Assert.True(actual.IsFailure);
        Assert.Equal(UserErrors.WrongPassword, actual.Error);
        _passwordHasherServiceMock.Verify(x => x.HashPassword(command.NewPassword), Times.Never);
    }

    private void SetupRepoMock(User user) =>
        _userRepositoryMock
            .Setup(x => x.GetByIdIncludeAccountAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
}