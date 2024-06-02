using Nutrifica.Infrastructure.Authentication;

namespace Nutrifica.Infrastructure.UnitTests.Utilities;

public class JwtSettingsFactory
{
    public static JwtSettings Create()
    {
        return new JwtSettings()
        {
            Audience = "audience",
            Issuer = "Issuer",
            AccessTokenExpiryMinutes = 5,
            SecretKey = "aaaaaaaaaaaaaaaaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbbb",
            RefreshTokenExpiryMinutes = 20
        };
    }
}