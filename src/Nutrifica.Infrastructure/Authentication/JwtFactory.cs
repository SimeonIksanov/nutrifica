using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;

namespace Nutrifica.Infrastructure.Authentication;

public class JwtFactory : IJwtFactory
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtFactory(JwtSettings jwtSettings, IDateTimeProvider dateTimeProvider)
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
            _dateTimeProvider.UtcNow,
            _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
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
            new Claim(ClaimTypes.NameIdentifier, user.Account.Username),
            new Claim(ClaimTypes.Name, user.FirstName.Value),
            new Claim(ClaimTypes.Surname, user.LastName.Value),
            new Claim(ClaimTypes.Role, ((int)user.Role).ToString())
        };
    }
}
