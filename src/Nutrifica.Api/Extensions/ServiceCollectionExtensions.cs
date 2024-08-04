using Nutrifica.Api.Services;
using Nutrifica.Application.Interfaces.Services;

namespace Nutrifica.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddControllers();
        services.ConfigureCors();
        services.AddScopedServices();
        return services;
    }

    private static void ConfigureCors(this IServiceCollection services)
    {
        services
            .AddCors(config =>
            {
                config.AddPolicy("Open",
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });

                config.AddPolicy("Specific",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost", "http://localhost")
                            .AllowAnyMethod()
                            .AllowAnyMethod();
                    });
            });
    }

    private static void AddScopedServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor().AddScoped<ICurrentUserService, CurrentUserService>();
    }
}