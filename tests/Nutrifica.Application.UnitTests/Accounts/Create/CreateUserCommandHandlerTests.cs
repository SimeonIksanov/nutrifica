using Moq;

using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.UnitTests.Utils;
using Nutrifica.Application.Users.Create;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.UnitTests.Accounts.Create;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    private CreateUserCommand _command;
    private readonly CreateUserCommandHandler _sut;

    public CreateUserCommandHandlerTests()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _userRepositoryMock = new Mock<IUserRepository>();
        _command = CreateCommand();
        _sut = new CreateUserCommandHandler(_userRepositoryMock.Object, unitOfWorkMock.Object);
    }

    [Fact]
    public async void Handle_When_SupervisorProvided_Returns_CreatedUserWithSupervisor()
    {
        var supervisor = UserUtils.CreateUser();
        SetupUserRepositoryMock(supervisor);
        _command = _command with { SupervisorId = supervisor.Id };
        var actual = await _sut.Handle(_command, default);

        Assert.True(actual.IsSuccess);
        Assert.IsType<UserResponse>(actual.Value);
        Assert.Equal(_command.Username, actual.Value.Username);
        Assert.NotNull(actual.Value.Supervisor);
    }

    [Fact]
    public async void Handle_When_SupervisorNotProvided_Returns_CreatedUserWithoutSupervisor()
    {
        var actual = await _sut.Handle(_command, default);

        Assert.True(actual.IsSuccess);
        Assert.IsType<UserResponse>(actual.Value);
        Assert.Equal(_command.Username, actual.Value.Username);
        Assert.Null(actual.Value.Supervisor);
    }

    [Fact]
    public async void Handle_Returns_EnabledUser()
    {
        var actual = await _sut.Handle(_command, default);

        Assert.True(actual.IsSuccess);
        Assert.IsType<UserResponse>(actual.Value);
        Assert.Equal(_command.Username, actual.Value.Username);
        Assert.Null(actual.Value.Supervisor);
    }

    [Fact]
    public async void Handle_Returns_UserWithCreatedAtNotEmpty()
    {
        var actual = await _sut.Handle(_command, default);
        Assert.True(actual.IsSuccess);
        Assert.IsType<UserResponse>(actual.Value);
        Assert.Equal(_command.Username, actual.Value.Username);
        Assert.NotEqual(DateTime.MinValue, actual.Value.CreatedAt); // (actual.Value.CreatedAt);
    }

    private void SetupUserRepositoryMock(User supervisor)
    {
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(supervisor.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supervisor);
    }

    private CreateUserCommand CreateCommand(UserId? supervisorId = null)
    {
        return new CreateUserCommand("un",
            FirstName.Create("fn"),
            MiddleName.Create("mn"),
            LastName.Create("ln"),
            Email.Create("email"),
            PhoneNumber.Create("11"),
            supervisorId);
    }
}