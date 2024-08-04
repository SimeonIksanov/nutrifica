using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.GetById;

public class GetClientQueryHandler : IQueryHandler<GetClientQuery, ClientResponse>
{
    private readonly IClientRepository _clientRepository;

    public GetClientQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<ClientResponse>> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        var clientModel = await _clientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (clientModel is null)
        {
            return Result.Failure<ClientResponse>(ClientErrors.ClientNotFound);
        }

        return Result.Success(clientModel.ToClientResponse());
    }
}