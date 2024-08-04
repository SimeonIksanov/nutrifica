using System.ComponentModel.DataAnnotations;

using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserUpdateRequest
{
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Логин не может быть пустым")]
    [MaxLength(50, ErrorMessage = "Максимальная длина логина 50 символов")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Имя не может быть пустым")]
    [MaxLength(50, ErrorMessage = "Максимальная длина имени 50 символов")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина отчества 50 символов")]
    public string MiddleName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина фамилии 50 символов")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия не может быть пустой")]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Максимальная длина email 50 символов")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "Максимальная длина номера телефона 30 символов")]
    public string PhoneNumber { get; set; } = string.Empty;

    public UserRole Role { get; set; }
    public bool Enabled { get; set; }
    public string DisableReason { get; set; } = string.Empty;
    public Guid? SupervisorId { get; set; } = null;
}