using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Models.Authentication;
using Nutrifica.Domain.UserAggregate;

namespace Nutrifica.Infrastructure.Authentication;

public class JwtFactory : IJwtFactory
{
    private readonly IDateTimeService _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtFactory(JwtSettings jwtSettings, IDateTimeService dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettings;
    }

    public string GenerateAccessToken(User user)
    {
        var securityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            GetClaims(user),
            _dateTimeProvider.UtcNow.DateTime,
            _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes).DateTime,
            GetSigningCredentials()
        );
        var token = new JwtSecurityTokenHandler()
            .WriteToken(securityToken);

        return token;
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            CreatedByIp = ipAddress,
            CreatedAt = _dateTimeProvider.UtcNow,
            Expires = _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiryMinutes),
        };

        return refreshToken;
    }

    private SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(
            SecurityKeyProvider.GetSecurityKey(_jwtSettings),
            SecurityAlgorithms.HmacSha256);
    }

    private IEnumerable<Claim> GetClaims(User user)
    {
        return new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.Role, ((int)user.Role).ToString())
        };
    }
}