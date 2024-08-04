using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.Clients;

public record ClientCreateRequest
{
    [MaxLength(50, ErrorMessage = "Максимальная длина имени 50 символов")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина отчества 50 символов")]
    public string MiddleName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина фамилии 50 символов")]
    public string LastName { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Номер телефона не может быть пустой")]
    public string PhoneNumber { get; set; } = string.Empty;

    public AddressDto Address { get; set; } = new();
    public string Comment { get; set; } = string.Empty;
}