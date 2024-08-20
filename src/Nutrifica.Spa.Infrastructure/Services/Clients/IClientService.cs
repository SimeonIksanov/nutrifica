using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Clients;

public interface IClientService
{
    Task<IResult<ClientDto>> CreateAsync(ClientCreateDto dto, CancellationToken cancellationToken);
    Task<IResult<ClientDto>> UpdateAsync(ClientUpdateDto dto, CancellationToken cancellationToken);
    Task<IResult<PagedList<ClientDto>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<IResult<ClientDto>> GetByIdAsync(Guid clientId, CancellationToken cancellationToken);

    Task<IResult<PagedList<PhoneCallDto>>> GetPhoneCallsAsync(Guid clientId, QueryParams queryParams,
        CancellationToken cancellationToken);

    Task<IResult<PhoneCallDto>> CreatePhoneCallAsync(Guid clientId, PhoneCallCreateDto dto,
        CancellationToken cancellationToken);

    Task<IResult<PhoneCallDto>> UpdatePhoneCallAsync(Guid clientId, PhoneCallUpdateDto dto,
        CancellationToken cancellationToken);

    Task<IResult> DeletePhoneCallAsync(Guid clientId, int phoneCallId, CancellationToken cancellationToken);
}