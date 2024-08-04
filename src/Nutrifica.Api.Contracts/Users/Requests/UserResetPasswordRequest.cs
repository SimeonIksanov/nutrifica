using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserResetPasswordRequest
{
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль не может быть пустым")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина должна быть от 6 до 50 символов.")]
    public string NewPassword { get; set; } = string.Empty;
}