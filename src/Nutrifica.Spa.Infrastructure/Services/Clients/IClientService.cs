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
}