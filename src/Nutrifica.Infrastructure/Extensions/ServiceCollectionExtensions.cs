using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Infrastructure.Authentication;
using Nutrifica.Infrastructure.Services;

namespace Nutrifica.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddJwtAuthentication(configuration);
        services.AddServices();
        return services;
    }

    private static void AddJwtAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
        
        services.AddSingleton(jwtSettings);
        services.AddSingleton<IJwtFactory, JwtFactory>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = SecurityKeyProvider.GetSecurityKey(jwtSettings)
            };
        });
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeService, DateTimeService>();
    }
}