using Moq;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.PhoneCalls.Create;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.UnitTests.Clients.PhoneCalls;

public class CreatePhoneCallCommandHandlerTest
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IPhoneCallRepository> _phoneCallRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    // private CreatePhoneCallCommand _command;
    private readonly CreatePhoneCallCommandHandler _sut;

    public CreatePhoneCallCommandHandlerTest()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _phoneCallRepositoryMock = new Mock<IPhoneCallRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _sut = new CreatePhoneCallCommandHandler(_clientRepositoryMock.Object, _phoneCallRepositoryMock.Object,_userRepositoryMock.Object, unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Returns_NewPhoneCall()
    {
        var command =
            new CreatePhoneCallCommand(ClientId.CreateUnique(), "some Comment created");
        var result = await _sut.Handle(command, default);

        Assert.True(result.IsFailure);
    }
}