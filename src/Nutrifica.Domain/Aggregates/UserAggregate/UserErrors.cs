using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain;

public static class UserErrors
{
    public static Error BadLoginOrPassword = new Error(
        "User.BadLoginOrPassword",
        "Bad username or password.");

    public static Error UserNotFound = new Error(
        "User.UserNotFound",
        "User not found.");

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
}
