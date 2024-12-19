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
                // client.ManagerIds
            });
        // var filtered = _sieveProcessor.Apply(queryParams.ToSieveModel(), query, applyPagination: false);
        // var totalCount = await filtered.CountAsync(cancellationToken);
        // var clients = await _sieveProcessor
        //     .Apply(queryParams.ToSieveModel(), query, applyFiltering: false, applySorting: false)
        //     .ToListAsync(cancellationToken);
        // var managerIds = clients.SelectMany(c => c.ManagerIds);
        // var managers = await _context.Users
        //     .Where(user => managerIds.Contains(user.Id))
        //     .Select(user => new UserShortModel(user.Id, user.FirstName, user.MiddleName, user.LastName))
        //     .ToListAsync(cancellationToken);
        // var clientModels = clients
        //     .Select(client => new ClientModel
        //     {
        //         Id = client.Id,
        //         FirstName = client.FirstName,
        //         MiddleName = client.MiddleName,
        //         LastName = client.LastName,
        //         Address = client.Address,
        //         Comment = client.Comment,
        //         PhoneNumber = client.PhoneNumber,
        //         Source = client.Source,
        //         State = client.State,
        //         CreatedOn = client.CreatedOn,
        //         Managers = client
        //             .ManagerIds
        //             .Join(managers,
        //                 managerId => managerId,
        //                 user => user.Id,
        //                 (_, uModel) => uModel)
        //             .ToList()
        //     })
        //     .ToList();

        // return PagedList<ClientModel>.Create(clientModels, queryParams.Page ?? 1,
        //     queryParams.PageSize ?? clientModels.Count, totalCount);
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

    // public async Task<PagedList<PhoneCallModel>> GetPhoneCallsAsync(ClientId id, QueryParams queryParams,
    //     CancellationToken cancellationToken)
    // {
    //     var query = _context
    //         .Set<Client>()
    //         .Where(c => c.Id.Equals(id))
    //         .SelectMany(c => c.PhoneCalls)
    //         .Select(phoneCall => new { phoneCall.Id, phoneCall.CreatedOn, phoneCall.CreatedBy, phoneCall.Comment });
    //     var pagedCals = await query
    //         .SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
    //     var userIds = pagedCals.Items.Select(call => call.CreatedBy).ToList();
    //     var userModels = await _context
    //         .Users
    //         .Where(user => userIds.Any(uid => uid == user.Id))
    //         .Select(user => new UserShortModel(user.Id, user.FirstName, user.MiddleName, user.LastName))
    //         .ToListAsync(cancellationToken);
    //
    //     var phoneCallModels = pagedCals
    //         .Items
    //         .Join(userModels,
    //             phoneCall => phoneCall.CreatedBy,
    //             user => user.Id,
    //             (phoneCall, uModel) => new PhoneCallModel(phoneCall.Id, phoneCall.CreatedOn, uModel, phoneCall.Comment))
    //         .ToList();
    //     return PagedList<PhoneCallModel>.Create(phoneCallModels,
    //         queryParams.Page ?? 1,
    //         queryParams.PageSize ?? phoneCallModels.Count,
    //         pagedCals.TotalCount);
    // }
}