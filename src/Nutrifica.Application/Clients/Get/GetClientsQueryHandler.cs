using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Get;

public class GetClientsQueryHandler : IQueryHandler<GetClientsQuery, PagedList<ClientResponse>>
{
    private readonly IClientRepository _clientRepository;

    public GetClientsQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<PagedList<ClientResponse>>> Handle(GetClientsQuery request,
        CancellationToken cancellationToken)
    {
        var clientPagedList = await _clientRepository
            .GetByFilterAsync(request.QueryParams, cancellationToken);

        var responseList = PagedList<ClientResponse>.Create(
            clientPagedList.Items.Select(x => x.ToClientResponse()).ToList(),
            clientPagedList.Page,
            clientPagedList.PageSize,
            clientPagedList.TotalCount);

        return Result.Success(responseList);
    }
}