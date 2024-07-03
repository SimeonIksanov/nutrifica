using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain.Aggregates.UserAggregate;

public static class UserErrors
{
    public static Error BadLoginOrPassword = new Error(
        "User.BadLoginOrPassword",
        "Bad username or password.");
    
    public static Error WrongPassword = new Error(
        "User.WrongPassword",
        "Wrong password specified.");

    public static Error UserNotFound = new Error(
        "User.UserNotFound",
        "User not found.");

    public static Error SupervisorNotFound = new Error(
        "User.SupervisorNotFound",
        "Supervisor not found.");

    public static Error BadJwt = new Error(
        "User.BadJwt",
        "Bad JWT provided."
    );

    public static Error RefreshTokenNotFound = new Error(
        "User.RefreshTokenNotFound",
        "Provided refresh token not found."
    );

    public static Error RefreshTokenNotActive = new Error(
        "User.RefreshTokenNotActive",
        "Provided refresh token expired or revoked."
    );

    public static Error Disabled = new Error(
        "User.Disabled",
        "User is disabled."
    );

    public static Error DisableReasonNotSpecified = new Error(
        "User.DisableReasonNotSpecified",
        "Disable reason not specified."
    );

    public static Error UsernameIsAlreadyInUse = new Error(
        "User.UsernameIsAlreadyInUse",
        "Username is already in use.");
}