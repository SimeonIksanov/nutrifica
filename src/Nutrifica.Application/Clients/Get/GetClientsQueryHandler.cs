using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Get;

public class GetClientsQueryHandler : IQueryHandler<GetClientsQuery, PagedList<ClientDto>>
{
    private readonly IClientRepository _clientRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetClientsQueryHandler(IClientRepository clientRepository, ICurrentUserService currentUserService)
    {
        _clientRepository = clientRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PagedList<ClientDto>>> Handle(GetClientsQuery request,
        CancellationToken cancellationToken)
    {
        var clientPagedList = await _clientRepository
            .GetByFilterAsync(request.QueryParams, cancellationToken);

        var responseList = PagedList<ClientDto>.Create(
            clientPagedList.Items.Select(x => x.ToClientDto()).ToList(),
            clientPagedList.Page,
            clientPagedList.PageSize,
            clientPagedList.TotalCount);

        return Result.Success(responseList);
    }
}