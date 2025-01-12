using System.Security.Claims;

using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        // UserId = UserId.Empty;
        UserId = null;

        if (!IsAuthenticated(httpContextAccessor))
        {
            return;
        }

        if (Guid.TryParse(ExtractClaimValue(httpContextAccessor, ClaimTypes.Sid), out Guid guid))
            UserId = UserId.Create(guid);
    }

    public UserId? UserId { get; }

    private bool IsAuthenticated(IHttpContextAccessor httpContextAccessor) =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    private string? ExtractClaimValue(IHttpContextAccessor httpContextAccessor, string claimType) =>
        httpContextAccessor
            .HttpContext?
            .User
            .Claims.FirstOrDefault(x => x.Type.Equals(claimType))?
            .Value;
}