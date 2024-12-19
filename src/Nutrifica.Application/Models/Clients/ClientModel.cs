using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Models.Clients;

public record ClientModel
{
    public ClientId Id { get; set; } = null!;
    public FirstName FirstName { get; set; } = null!;
    public MiddleName MiddleName { get; set; } = null!;
    public LastName LastName { get; set; } = null!;
    public AddressModel Address { get; set; } = null!;
    public Comment Comment { get; set; } = null!;
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public string Source { get; set; } = string.Empty;
    public State State { get; set; }
    public DateTime CreatedOn { get; set; }
    public ICollection<UserShortModel> Managers { get; set; } = null!;
}