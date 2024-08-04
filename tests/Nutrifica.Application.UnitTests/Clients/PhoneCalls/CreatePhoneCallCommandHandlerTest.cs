using Moq;

using Nutrifica.Application.Clients.CreatePhoneCall;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.UnitTests.Clients.PhoneCalls;

public class CreatePhoneCallCommandHandlerTest
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;

    // private CreatePhoneCallCommand _command;
    private readonly CreatePhoneCallCommandHandler _sut;

    public CreatePhoneCallCommandHandlerTest()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _sut = new CreatePhoneCallCommandHandler(_clientRepositoryMock.Object, unitOfWorkMock.Object);
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