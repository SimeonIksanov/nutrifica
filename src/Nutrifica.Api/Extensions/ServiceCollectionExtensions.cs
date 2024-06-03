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
        services.AddCors(options =>
            options.AddDefaultPolicy(config =>
                config
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));
    }
}