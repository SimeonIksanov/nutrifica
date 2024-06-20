using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    public void Add(Client client) => throw new NotImplementedException();

    public Task<Client?> GetByIdAsync(ClientId clientId, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<IPagedList<ClientModel>> GetByFilterAsync(ClientQueryParams requestQueryParams, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<ClientDetailedModel?> GetDetailedByIdAsync(ClientId id, CancellationToken cancellationToken) => throw new NotImplementedException();
}