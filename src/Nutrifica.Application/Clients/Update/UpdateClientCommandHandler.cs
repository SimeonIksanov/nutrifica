using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Update;

public class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand, ClientDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<ClientDto>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetEntityByIdAsync(request.Id, cancellationToken);
        if (client is null)
        {
            return Result.Failure<ClientDto>(ClientErrors.ClientNotFound);
        }

        MapToClient(request, client);
        client.SetManagerIds(request.ManagerIds);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = client.ToClientDto();
        return Result.Success(dto);
    }

    private static void MapToClient(UpdateClientCommand request, Client client)
    {
        client.FirstName = request.FirstName;
        client.MiddleName = request.MiddleName;
        client.LastName = request.LastName;
        client.Address = request.Address;
        client.Comment = request.Comment;
        client.PhoneNumber = request.PhoneNumber;
        client.Source = request.Source;
        client.State = request.State;
    }
}