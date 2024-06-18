using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public static class ClientErrors
{
    public static Error ClientNotFound = new Error(
        "User.UserNotFound",
        "User not found.");
}