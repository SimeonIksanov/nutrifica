using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;

namespace Nutrifica.Application.PhoneCalls.Update;

public record UpdatePhoneCallCommand(ClientId ClientId, PhoneCallId PhoneCallId, string Comment) : ICommand<PhoneCallDto>;