using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;

namespace Nutrifica.Application.Clients.Get;

public record GetClientsQuery(
    object sieveModel) : IQuery<IPagedList<ClientResponse>>;