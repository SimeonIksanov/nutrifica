using System.Security.Claims;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;

namespace Nutrifica.Application.Interfaces.Services;

public interface IJwtFactory
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(string ipAddress);
}
