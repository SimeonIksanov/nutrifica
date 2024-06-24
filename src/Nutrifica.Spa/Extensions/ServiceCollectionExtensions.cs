using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor;
using MudBlazor.Services;

using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services
            .AddHttpClients(builder)
            .AddLocalStorage()
            .AddMudBlazor()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<NutrificaAuthenticationStateProvider>()
            .AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<NutrificaAuthenticationStateProvider>())
            .AddAuthorizationCore();
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

    private static IServiceCollection AddLocalStorage(this IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
        return services;
    }

    private static IServiceCollection AddMudBlazor(this IServiceCollection services)
    {
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
        return services;
    }
}