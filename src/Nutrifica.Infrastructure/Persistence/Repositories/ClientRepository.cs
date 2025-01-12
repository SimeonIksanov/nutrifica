using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Infrastructure.Services.SortAndFilter;
using Nutrifica.Shared.Wrappers;

using Sieve.Services;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

// TODO 2 запроса в бд потому что не получилось в один.. strongly typed IDs с OWNED типами работает странно
public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;
    private readonly ISieveProcessor _sieveProcessor;

    public ClientRepository(AppDbContext context, ISieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public void Add(Client client)
    {
        _context
            .Set<Client>()
            .Add(client);
    }

    public Task<Client?> GetEntityByIdAsync(ClientId clientId, CancellationToken ct = default)
    {
        return _context
            .Set<Client>()
            .FirstOrDefaultAsync(x => x.Id == clientId);
    }

    public async Task<ClientModel?> GetByIdAsync(ClientId clientId, CancellationToken ct = default)
    {
        // TODO урать дублированый код
        var clientModel = await _context
            .Clients
            .AsNoTracking()
            .Select(client => new ClientModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                MiddleName = client.MiddleName,
                LastName = client.LastName,
                Address = new AddressModel
                {
                    City = client.Address.City,
                    Comment = client.Address.Comment,
                    Country = client.Address.Country,
                    Region = client.Address.Region,
                    Street = client.Address.Street,
                    ZipCode = client.Address.ZipCode
                },
                Comment = client.Comment,
                PhoneNumber = client.PhoneNumber,
                Source = client.Source,
                State = client.State,
                CreatedOn = client.CreatedOn,
                // client.ManagerIds
            })
            .FirstOrDefaultAsync(client => client.Id == clientId, ct);
        // if (client is null)
        //     return null;
        // var managerIds = client.ManagerIds;
        // var managers = await _context.Users
        //     .Where(user => managerIds.Contains(user.Id))
        //     .Select(user => new UserShortModel(user.Id, user.FirstName, user.MiddleName, user.LastName))
        //     .ToListAsync(ct);
        // var clientModel = new ClientModel
        // {
        //     Id = client.Id,
        //     FirstName = client.FirstName,
        //     MiddleName = client.MiddleName,
        //     LastName = client.LastName,
        //     Address = client.Address,
        //     Comment = client.Comment,
        //     PhoneNumber = client.PhoneNumber,
        //     Source = client.Source,
        //     State = client.State,
        //     CreatedOn = client.CreatedOn,
        //     Managers = managers
        // };

        return clientModel;
    }

    public async Task<PagedList<ClientModel>> GetByFilterAsync(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var query = _context
            .Clients
            .AsNoTracking()
            .Select(client => new ClientModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                MiddleName = client.MiddleName,
                LastName = client.LastName,
                Address = new AddressModel
                {
                    City = client.Address.City,
                    Comment = client.Address.Comment,
                    Country = client.Address.Country,
                    Region = client.Address.Region,
                    Street = client.Address.Street,
                    ZipCode = client.Address.ZipCode
                },
                Comment = client.Comment,
                PhoneNumber = client.PhoneNumber,
                Source = client.Source,
                State = client.State,
                CreatedOn = client.CreatedOn,
            });
        var pagedList = await query
            .SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }

    public async Task<bool> HasClientWithIdAsync(ClientId id, CancellationToken cancellationToken)
    {
        return await _context
            .Set<Client>()
            .AnyAsync(client => client.Id.Equals(id), cancellationToken);
    }
}