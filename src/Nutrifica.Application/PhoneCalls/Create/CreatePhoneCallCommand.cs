using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.PhoneCalls.Create;

public record CreatePhoneCallCommand(ClientId clientId, string Comment) : ICommand<PhoneCallDto>;