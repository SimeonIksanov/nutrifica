using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Nutrifica.Spa.Extensions;

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

        await builder.Build().RunAsync();
    }
}