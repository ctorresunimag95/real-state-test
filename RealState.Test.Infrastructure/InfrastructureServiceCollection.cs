using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealState.Test.Domain.Property;
using RealState.Test.Infrastructure.ImageStore;
using RealState.Test.Infrastructure.Persistence;

namespace RealState.Test.Infrastructure;

public static class InfrastructureServiceCollection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddSingleton<IImageStoreProvider, ImageStoreProvider>();

        return services;
    }
}