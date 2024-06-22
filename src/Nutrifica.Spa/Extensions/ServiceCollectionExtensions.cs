using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services
            .AddServices()
            .AddHttpClients(builder);

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>();
        return services;
    }

    private static IServiceCollection AddHttpClients(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration.GetSection("backend")["uri"] ??
                                  throw new KeyNotFoundException("No backend URI specified"))
        });
        // services.AddHttpClient<AuthenticationService>(client =>
        // {
        //     client.BaseAddress = new Uri(builder.Configuration.GetSection("backend")["uri"] ?? throw new KeyNotFoundException("No backend URI specified"));
        // });

        services.AddHttpClient("blazorBackend", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        });

        return services;
    }
}