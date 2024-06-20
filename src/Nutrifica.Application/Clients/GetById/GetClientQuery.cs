using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.GetById;

public record GetClientQuery(ClientId Id) : IQuery<ClientDetailedResponse>;

public class GetClientQueryHandler : IQueryHandler<GetClientQuery, ClientDetailedResponse>
{
    private readonly IClientRepository _clientRepository;

    public GetClientQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<ClientDetailedResponse>> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        var clientModel = await _clientRepository.GetDetailedByIdAsync(request.Id, cancellationToken);
        if (clientModel is null)
        {
            return Result.Failure<ClientDetailedResponse>(ClientErrors.ClientNotFound);
        }

        return Result.Success(clientModel.ToClientResponse());
    }
}