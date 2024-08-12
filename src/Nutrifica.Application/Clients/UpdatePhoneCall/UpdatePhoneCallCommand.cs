using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Clients.UpdatePhoneCall;

public record UpdatePhoneCallCommand(ClientId ClientId, PhoneCallId PhoneCallId, string Comment) : ICommand<PhoneCallResponse>;