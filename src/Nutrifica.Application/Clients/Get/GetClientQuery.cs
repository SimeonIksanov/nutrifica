using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.Get;

public record GetClientsQuery(QueryParams QueryParams) : IQuery<PagedList<ClientResponse>>;