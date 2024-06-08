using System.Security.Claims;
using Nutrifica.Application.Models.Authentication;
using Nutrifica.Domain.Aggregates.UserAggregate;

namespace Nutrifica.Application.Interfaces.Services;

public interface IJwtFactory
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(string ipAddress);
}
