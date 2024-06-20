namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserChangePasswordRequest(
    Guid Id,
    string CurrentPassword,
    string NewPassword);