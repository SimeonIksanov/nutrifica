using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public static class PhoneCallErrors
{
    public static Error PhoneCallNotFound = new Error("PhoneCall.PhoneCallNotFound", "PhoneCall not found.");
}