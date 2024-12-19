using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.PhoneCalls;

public interface IPhoneCallService
{
    Task<IResult<PagedList<PhoneCallDto>>> GetPhoneCallsAsync(Guid clientId, QueryParams queryParams,
        CancellationToken cancellationToken);

    Task<IResult<PhoneCallDto>> CreatePhoneCallAsync(Guid clientId, PhoneCallCreateDto dto,
        CancellationToken cancellationToken);

    Task<IResult<PhoneCallDto>> UpdatePhoneCallAsync(Guid clientId, PhoneCallUpdateDto dto,
        CancellationToken cancellationToken);

    Task<IResult> DeletePhoneCallAsync(Guid clientId, Guid phoneCallId, CancellationToken cancellationToken);
}