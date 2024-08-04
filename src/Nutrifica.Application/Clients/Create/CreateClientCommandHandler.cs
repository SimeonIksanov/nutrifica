using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Create;

public class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, ClientResponse>
{
    private readonly IClientRepository _clientRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ClientResponse>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = Client.Create(
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.PhoneNumber,
            request.Address,
            request.Comment,
            _currentUserService.UserId,
            request.Source);

        _clientRepository.Add(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = client.ToClientResponse();
        return Result.Success(dto);
    }
}