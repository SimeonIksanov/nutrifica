using Microsoft.AspNetCore.Authorization;

using Nutrifica.Application.Authorization.Requirements;
using Nutrifica.Domain.Aggregates.ClientAggregate;

namespace Nutrifica.Application.Authorization;

public class ClientAuthorizationHandler : AuthorizationHandler<ClientManagerRequirement, Client>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ClientManagerRequirement requirement,
        Client resource)
    {
        return Task.CompletedTask;
    }
}