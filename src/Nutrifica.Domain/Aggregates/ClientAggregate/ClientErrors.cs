using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public static class ClientErrors
{
    public static Error ClientNotFound = new Error( "Client.ClientNotFound", "Client not found.");
}