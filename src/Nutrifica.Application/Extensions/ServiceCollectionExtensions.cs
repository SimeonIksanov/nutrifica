using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nutrifica.Application.CommandAndQueries.Common.Behaviors;

namespace Nutrifica.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}