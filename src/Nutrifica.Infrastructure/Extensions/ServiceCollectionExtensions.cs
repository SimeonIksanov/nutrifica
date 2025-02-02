using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Infrastructure.Authentication;
using Nutrifica.Infrastructure.Persistence;
using Nutrifica.Infrastructure.Services;

namespace Nutrifica.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddJwtAuthentication(configuration)
            .AddPersistence()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        ConfigurationManager configuration)
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

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IDateTimeService, DateTimeService>()
            .AddScoped<IPasswordHasherService, PasswordHasherService>();
        
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseInMemoryDatabase("InMemory"));
        
        return services;
    }
}