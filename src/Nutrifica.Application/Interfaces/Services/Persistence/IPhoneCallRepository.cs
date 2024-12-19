using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.PhoneCalls;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IPhoneCallRepository
{
    void Add(PhoneCall phoneCall);
    // Task<PagedList<PhoneCallModel>> GetByIdAsync(ClientId id, QueryParams queryParams, CancellationToken cancellationToken);
    Task<PagedList<PhoneCallModel>> GetByFilterAsync(ClientId id, QueryParams queryParams, CancellationToken cancellationToken);
    Task<PhoneCall?> GetEntityByIdAsync(PhoneCallId phoneCallId, CancellationToken cancellationToken);
}