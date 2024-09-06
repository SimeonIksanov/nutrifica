using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor;
using MudBlazor.Services;

using Nutrifica.Spa.Infrastructure.Services;
using Nutrifica.Spa.Infrastructure.Services.Authentication;
using Nutrifica.Spa.Infrastructure.Services.Clients;
using Nutrifica.Spa.Infrastructure.Services.Orders;
using Nutrifica.Spa.Infrastructure.Services.Products;
using Nutrifica.Spa.Infrastructure.Services.Users;
using Nutrifica.Spa.MiddleWares;

namespace Nutrifica.Spa.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientServices(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services
            .AddHttpClients(builder)
            .AddDelegateHandlers()
            .AddLocalStorage()
            .AddMudBlazor()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IClientService, ClientService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<NutrificaAuthenticationStateProvider>()
            .AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<NutrificaAuthenticationStateProvider>())
            .AddAuthorizationCore()
            .AddScoped<ITokenService, TokenService>();
        return services;
    }

    private static IServiceCollection AddHttpClients(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddHttpClient("apiBackend", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetSection("backend")["uri"] ??
                                             throw new KeyNotFoundException("No backend URI specified"));
            })
            .AddHttpMessageHandler<TokenRefreshDelegateHandler>()
            .AddHttpMessageHandler<JwtInjectorDelegateHandler>();

        services.AddHttpClient("apiBackendWoHandlers", config =>
            config.BaseAddress = new Uri(builder.Configuration.GetSection("backend")["uri"] ??
                                         throw new KeyNotFoundException("No backend URI specified")));

        services.AddHttpClient("blazorBackend", client =>
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

        return services;
    }

    private static IServiceCollection AddDelegateHandlers(this IServiceCollection services)
    {
        services.AddTransient<TokenRefreshDelegateHandler>();
        services.AddTransient<JwtInjectorDelegateHandler>();
        // services.AddTransient<AuthorizationMessageHandler>();

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
            config.SnackbarConfiguration.ClearAfterNavigation = true;
        });
        return services;
    }
}