using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Client client)
    {
        _context
            .Set<Client>()
            .Add(client);
    }

    public Task<Client?> GetByIdAsync(ClientId clientId, CancellationToken ct = default)
    {
        return _context
            .Set<Client>()
            .FirstOrDefaultAsync(x => x.Id == clientId);
    }

    public Task<IPagedList<ClientModel>> GetByFilterAsync(ClientQueryParams requestQueryParams, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ClientDetailedModel?> GetDetailedByIdAsync(ClientId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}