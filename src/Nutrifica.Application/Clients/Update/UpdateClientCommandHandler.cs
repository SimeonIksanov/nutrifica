using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Update;

public class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand, UpdatedClientDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result<UpdatedClientDto>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (client is null)
        {
            return Result.Failure<UpdatedClientDto>(ClientErrors.ClientNotFound);
        }

        MapToClient(request, client);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = client.ToUpdatedClientDto();
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
    }
}