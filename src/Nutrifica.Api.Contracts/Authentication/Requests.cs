using System.ComponentModel.DataAnnotations;

namespace Nutrifica.Api.Contracts.Authentication;

public record TokenRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Имя пользователя не может быть пустым!")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль не может быть пустым!")]
    public string Password { get; set; } = string.Empty;
}

public record RefreshTokenRequest(
    string Jwt,
    string RefreshToken);

public record LogoutRequest(
    string Jwt,
    string RefreshToken);