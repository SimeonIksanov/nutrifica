using Moq;

using Nutrifica.Application.Accounts.SetPassword;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.UnitTests.Utils;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Application.UnitTests.Accounts.SetPassword;

public class SetPasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasherService> _passwordHasherServiceMock;

    private readonly SetPasswordCommandHandler _sut;

    public SetPasswordCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherServiceMock = new Mock<IPasswordHasherService>();

        _sut = new SetPasswordCommandHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object,
            _passwordHasherServiceMock.Object);
    }

    [Fact]
    public async void Handle_When_OwnPassChanged_Returns_Success()
    {
        var user = UserUtils.CreateUser();
        var command = new SetPasswordCommand(user.Id, "cur", "new");

        _userRepositoryMock
            .Setup(x => x.GetByIdWithPasswordHashAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
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
    public async void Handle_When_PassReset_Returns_Success()
    {
        var user = UserUtils.CreateUser();
        var command = new SetPasswordCommand(user.Id, "", "new");
        _userRepositoryMock
            .Setup(x => x.GetByIdWithPasswordHashAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasherServiceMock
            .Setup(x => x.HashPassword(command.NewPassword))
            .Returns(("newHash", "newSalt"));

        var actual = await _sut.Handle(command, default);

        Assert.True(actual.IsSuccess);
        Assert.Equal("newHash", user.Account.PasswordHash);
        Assert.Equal("newSalt", user.Account.Salt);
    }

    [Fact]
    public async void Handle_When_UserNotFound_Returns_UserNotFound()
    {
        var command = new SetPasswordCommand(UserId.CreateUnique(), "", "new");
        _userRepositoryMock
            .Setup(x => x.GetByIdWithPasswordHashAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        var actual = await _sut.Handle(command, default);

        Assert.True(actual.IsFailure);
        Assert.Equal(UserErrors.UserNotFound, actual.Error);
        _passwordHasherServiceMock.Verify(x => x.HashPassword(command.NewPassword), Times.Never);
    }

    [Fact]
    public async void Handle_When_WrongCurPass_Returns_WrongPassword()
    {
        var user = UserUtils.CreateUser();
        var command = new SetPasswordCommand(user.Id, "wrong", "new");
        _userRepositoryMock
            .Setup(x => x.GetByIdWithPasswordHashAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasherServiceMock
            .Setup(x => x.Verify(command.CurrentPassword, user.Account.PasswordHash, user.Account.Salt))
            .Returns(false);

        var actual = await _sut.Handle(command, default);

        Assert.True(actual.IsFailure);
        Assert.Equal(UserErrors.WrongPassword, actual.Error);
        _passwordHasherServiceMock.Verify(x => x.HashPassword(command.NewPassword), Times.Never);
    }
}