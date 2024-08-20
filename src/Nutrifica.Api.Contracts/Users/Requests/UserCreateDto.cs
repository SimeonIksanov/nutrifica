// ReSharper disable NotAccessedPositionalProperty.Global

using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserCreateDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Логин не может быть пустым")]
    [MaxLength(50, ErrorMessage = "Максимальная длина 50 символов")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Имя не может быть пустым")]
    [MaxLength(50, ErrorMessage = "Максимальная длина 50 символов")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина 50 символов")]
    public string MiddleName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина 50 символов")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия не может быть пустой")]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина 50 символов")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "Максимальная длина 30 символов")]
    public string PhoneNumber { get; set; } = string.Empty;

    public Guid? SupervisorId { get; set; }
}