using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Nutrifica.Spa.Extensions;
using Nutrifica.Spa.Infrastructure.Services.Authentication;
using Nutrifica.Spa.Infrastructure.Services.Storage;

namespace Nutrifica.Spa;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder
            .CreateDefault(args);

        builder
            .RootComponents
            .Add<App>("#app");

        builder
            .RootComponents
            .Add<HeadOutlet>("head::after");

        builder
            .Services
            .AddClientServices(builder);
        //
        builder
            .Services
            .AddScoped<IDataStorage,MemoryDataStorage>()
            .AddScoped<UserService>()
            .AddScoped<NutrificaAuthenticationStateProvider>()
            .AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<NutrificaAuthenticationStateProvider>())
            .AddAuthorizationCore();
        //

        await builder.Build().RunAsync();
    }
}