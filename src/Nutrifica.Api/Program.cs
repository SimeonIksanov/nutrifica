using Nutrifica.Api.Extensions;
using Nutrifica.Application.Extensions;
using Nutrifica.Infrastructure.Extensions;

namespace Nutrifica.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();

        builder
            .Services
            .AddPresentationLayer()
            .AddApplicationLayer()
            .AddInfrastructureLayer(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.UseSwagger();
            // app.UseSwaggerUI();
            app.SeedDevelopmentData();
            app.UseCors("Open");
        }
        else
        {
            app.UseCors("Specific");
        }

        app
            .UseHttpsRedirection()
            .UseAuthentication()
            .UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}