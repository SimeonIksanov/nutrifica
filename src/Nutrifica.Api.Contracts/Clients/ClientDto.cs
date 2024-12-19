using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Clients;

public record ClientDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public State State { get; set; }
    // public ICollection<UserShortDto> Managers { get; set; } = null!;
    public string FullName => string.Join(" ", LastName, FirstName, MiddleName);
}