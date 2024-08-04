using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.GetClientPhoneCalls;

public record GetClientPhoneCallsQuery(ClientId Id, QueryParams QueryParams) : IQuery<PagedList<PhoneCallResponse>>;