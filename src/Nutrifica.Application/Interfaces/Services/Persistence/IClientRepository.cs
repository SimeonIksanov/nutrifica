using Nutrifica.Application.Models.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IClientRepository
{
    void Add(Client client);
    Task<Client?> GetByIdAsync(ClientId clientId, CancellationToken ct = default);
    Task<IPagedList<ClientModel>> GetByFilterAsync(ClientQueryParams requestQueryParams, CancellationToken cancellationToken);
    Task<ClientDetailedModel?> GetDetailedByIdAsync(ClientId id, CancellationToken cancellationToken);

}
