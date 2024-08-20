using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.Users;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Infrastructure.Services.SortAndFilter;
using Nutrifica.Shared.Wrappers;

using Sieve.Services;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

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

    public Task<Client?> GetByIdAsync(ClientId clientId, CancellationToken ct = default)
    {
        return _context
            .Set<Client>()
            .FirstOrDefaultAsync(x => x.Id == clientId);
    }

    public async Task<PagedList<ClientModel>> GetByFilterAsync(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<Client>()
            .AsNoTracking()
            .Select(ClientToClientModel());
        var pagedList =
            await query.SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }


    public async Task<bool> HasClientWithIdAsync(ClientId id, CancellationToken cancellationToken)
    {
        return await _context
            .Set<Client>()
            .AnyAsync(client => client.Id.Equals(id), cancellationToken);
    }

    public async Task<PagedList<PhoneCallModel>> GetPhoneCallsAsync(ClientId id, QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        IQueryable<PhoneCallModel> query = _context
            .Set<Client>()
            .Where(c => c.Id.Equals(id))
            .SelectMany(c => c.PhoneCalls)
            .GroupJoin(
                _context.Set<User>(),
                phoneCall => phoneCall.CreatedBy,
                user => user.Id,
                (phoneCall, users) => new { phoneCall, users })
            .SelectMany(
                x => x.users.DefaultIfEmpty(),
                (x, user) => new PhoneCallModel(
                    x.phoneCall.Id,
                    x.phoneCall.CreatedOn,
                    Equals(user, null)
                        ? new UserShortModel(x.phoneCall.CreatedBy.Value, "unknown", "unknown", "unknown")
                        : new UserShortModel(user.Id.Value, user.FirstName.Value, user.MiddleName.Value,
                            user.LastName.Value),
                    x.phoneCall.Comment));
        var pagedList =
            await query.SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }

    private Expression<Func<Client,ClientModel>> ClientToClientModel() =>
        client => new ClientModel{
            Id = client.Id.Value,
            FirstName = client.FirstName.Value,
            MiddleName = client.MiddleName.Value,
            LastName = client.LastName.Value,
            Address = new()
            {
                City = client.Address.City,
                Comment = client.Address.Comment,
                Country = client.Address.Country,
                Region = client.Address.Region,
                Street = client.Address.Street,
                ZipCode = client.Address.ZipCode
            },
            Comment = client.Comment.Value,
            PhoneNumber = client.PhoneNumber.Value,
            Source = client.Source,
            State = client.State,
            CreatedOn = client.CreatedOn
        };
}