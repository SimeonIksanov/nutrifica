using System.ComponentModel.DataAnnotations;

using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Clients;

public record ClientUpdateDto
{
    public Guid Id { get; set; }

    [MaxLength(50, ErrorMessage = "Максимальная длина имени 50")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина отчества 50")]
    public string MiddleName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина фамилии 50")]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "Максимальная длина номера телефона 50")]
    public string PhoneNumber { get; set; } = string.Empty;

    public AddressDto Address { get; set; } = new();
    public string Comment { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public State State { get; set; }
    public ICollection<Guid> ManagerIds { get; set; } = null!;
}