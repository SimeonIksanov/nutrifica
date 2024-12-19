using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Models.Clients;

public record ClientShortModel
{
    public ClientId Id { get; set; } = null!;
    public FirstName FirstName { get; set; } = null!;
    public MiddleName MiddleName { get; set; } = null!;
    public LastName LastName { get; set; } = null!;
}