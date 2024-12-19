using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.PhoneCalls;
using Nutrifica.Application.Models.Users;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;
using Nutrifica.Infrastructure.Services.SortAndFilter;
using Nutrifica.Shared.Wrappers;

using Sieve.Services;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class PhoneCallRepository : IPhoneCallRepository
{
    private readonly AppDbContext _context;
    private readonly ISieveProcessor _sieveProcessor;

    public PhoneCallRepository(AppDbContext context, ISieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public void Add(PhoneCall phoneCall) => _context.Set<PhoneCall>().Add(phoneCall);

    public async Task<PagedList<PhoneCallModel>> GetByFilterAsync(ClientId clientId, QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var query = from call in _context.PhoneCalls.AsNoTracking()
            where call.ClientId == clientId
            join client in _context.Clients.AsNoTracking()
                on call.ClientId equals client.Id
            join user in _context.Users.AsNoTracking()
                on call.CreatedBy equals user.Id
            select new PhoneCallModel
            {
                Id = call.Id,
                Client = client == null
                    ? null
                    : new ClientShortModel
                    {
                        Id = client.Id,
                        FirstName = client.FirstName,
                        MiddleName = client.MiddleName,
                        LastName = client.LastName
                    },
                CreatedOn = call.CreatedOn,
                CreatedBy = user == null
                    ? null
                    : new UserShortModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        LastName = user.LastName
                    },
                Comment = call.Comment
            };
        var pagedList = await query
            .SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }

    public async Task<PhoneCall?> GetEntityByIdAsync(PhoneCallId phoneCallId, CancellationToken cancellationToken)
    {
        return await _context.PhoneCalls.FirstOrDefaultAsync(call => call.Id == phoneCallId, cancellationToken);
    }
}