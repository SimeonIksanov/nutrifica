using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Create;

public class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, CreatedClientDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
    }

    public async Task<Result<CreatedClientDto>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = Client.Create(
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.PhoneNumber,
            request.Address,
            request.Comment,
            request.CreatedBy,
            request.Source);

        _clientRepository.Add(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = client.ToCreatedClientDto();
        return Result.Success(dto);
    }
}