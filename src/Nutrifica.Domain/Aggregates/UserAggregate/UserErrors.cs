using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain;

public static class UserErrors
{
    public static Error BadLoginOrPassword = new Error(
        "User.BadLoginOrPassword",
        "Bad username or password");
}
