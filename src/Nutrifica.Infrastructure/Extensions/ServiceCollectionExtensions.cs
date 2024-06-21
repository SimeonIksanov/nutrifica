using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Nutrifica.Application.Abstractions.Clock;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Infrastructure.Authentication;
using Nutrifica.Infrastructure.Clock;
using Nutrifica.Infrastructure.Persistence;
using Nutrifica.Infrastructure.Persistence.Repositories;
using Nutrifica.Infrastructure.Services;
using Nutrifica.Infrastructure.Services.SortAndFilter;

using Sieve.Models;
using Sieve.Services;

namespace Nutrifica.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddJwtAuthentication(configuration)
            .AddPersistence()
            .AddServices()
            .AddSieveSortingAndFiltering(configuration);

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
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<IPasswordHasherService, PasswordHasherService>();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseInMemoryDatabase("InMemory"));

        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IClientRepository, ClientRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }

    private static IServiceCollection AddSieveSortingAndFiltering(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<ISieveProcessor, CustomSieveProcessor>();
        services.Configure<SieveOptions>(configuration.GetSection(nameof(SieveOptions)));
        return services;
    }
}