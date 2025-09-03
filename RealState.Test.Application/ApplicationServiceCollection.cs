using Microsoft.Extensions.DependencyInjection;
using RealState.Test.Application.Property;

namespace RealState.Test.Application;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddProperty();

        return services;
    }
}