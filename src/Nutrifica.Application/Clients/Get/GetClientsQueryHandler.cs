using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Get;

public class GetClientsQueryHandler : IQueryHandler<GetClientsQuery, IPagedList<ClientResponse>>
{
    private readonly IClientRepository _clientRepository;

    public GetClientsQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<IPagedList<ClientResponse>>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clientPagedList = await _clientRepository
                .GetByFilterAsync(request.QueryParams, cancellationToken);

        var clientResponsePagedList = clientPagedList.ProjectItems(x => x.ToClientResponse());
        return Result.Success<IPagedList<ClientResponse>>(clientResponsePagedList);
    }
}