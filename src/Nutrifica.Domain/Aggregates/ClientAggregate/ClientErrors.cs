using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public static class ClientErrors
{
    public static Error ClientNotFound = new Error(
        "Client.UserNotFound",
        "Client not found.");
    public static Error PhoneCallNotFound = new Error("Client.ClientPhoneCallNotFound",
        "PhoneCall not found.");
}