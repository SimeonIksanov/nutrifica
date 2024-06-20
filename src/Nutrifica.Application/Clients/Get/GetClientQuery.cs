using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Application.Clients.Get;

public record GetClientsQuery(
    ClientQueryParams QueryParams) : IQuery<IPagedList<ClientResponse>>;