using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Get;

public record GetClientsQuery(
    object sieveModel) : IQuery<PagedList<ClientResponse>>;