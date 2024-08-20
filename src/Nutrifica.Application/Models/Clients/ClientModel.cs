using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Models.Clients;

public record ClientModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public AddressModel Address { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public State State { get; set; }
    public DateTime CreatedOn { get; set; }
}