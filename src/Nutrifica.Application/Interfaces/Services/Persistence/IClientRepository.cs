using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IClientRepository
{
    void Add(Client client);
    Task<bool> HasClientWithIdAsync(ClientId id, CancellationToken cancellationToken);
    Task<Client?> GetEntityByIdAsync(ClientId clientId, CancellationToken ct = default);
    Task<ClientModel?> GetByIdAsync(ClientId clientId, CancellationToken ct = default);
    Task<PagedList<ClientModel>> GetByFilterAsync(QueryParams queryParams, CancellationToken cancellationToken);

}