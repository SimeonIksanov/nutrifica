namespace Nutrifica.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddControllers();
        services.ConfigureCors();
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
}