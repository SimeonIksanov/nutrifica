using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.PhoneCalls.Get;

public record GetClientPhoneCallsQuery(ClientId Id, QueryParams QueryParams) : IQuery<PagedList<PhoneCallDto>>;