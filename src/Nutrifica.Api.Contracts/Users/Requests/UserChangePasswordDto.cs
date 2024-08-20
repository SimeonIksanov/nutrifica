using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserChangePasswordDto
{
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль не может быть пустым")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль не может быть пустым")]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}