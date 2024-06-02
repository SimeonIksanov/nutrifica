using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Nutrifica.Infrastructure.Authentication;

internal static class SecurityKeyProvider
{
    public static SecurityKey GetSecurityKey(JwtSettings jwtSettings)
    {
        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
        return new SymmetricSecurityKey(key);
    }
}