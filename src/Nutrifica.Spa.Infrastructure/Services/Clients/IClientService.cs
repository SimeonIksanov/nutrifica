using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Clients;

public interface IClientService
{
    Task<IResult<ClientResponse>> CreateAsync(ClientCreateRequest request, CancellationToken cancellationToken);
    Task<IResult<ClientResponse>> UpdateAsync(ClientUpdateRequest request, CancellationToken cancellationToken);
    Task<IResult<PagedList<ClientResponse>>> GetAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<IResult<ClientResponse>> GetByIdAsync(Guid clientId, CancellationToken cancellationToken);

    Task<IResult<PagedList<PhoneCallResponse>>> GetPhoneCallsAsync(Guid clientId, QueryParams queryParams,
        CancellationToken cancellationToken);

    Task<IResult<PhoneCallResponse>> CreatePhoneCallAsync(Guid clientId, PhoneCallCreateRequest request,
        CancellationToken cancellationToken);

    Task<IResult<PhoneCallResponse>> UpdatePhoneCallAsync(Guid clientId, PhoneCallUpdateRequest request,
        CancellationToken cancellationToken);

    Task<IResult> DeletePhoneCallAsync(Guid clientId, int phoneCallId, CancellationToken cancellationToken);
}