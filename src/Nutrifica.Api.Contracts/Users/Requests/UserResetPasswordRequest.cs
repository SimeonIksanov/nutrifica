namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserResetPasswordRequest(
    Guid Id,
    string NewPassword);