using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Models.Users;

public record UserShortModel
{
    public UserId Id { get; set; } = null!;
    public FirstName FirstName { get; set; } = null!;
    public MiddleName MiddleName { get; set; } = null!;
    public LastName LastName { get; set; } = null!;
}
