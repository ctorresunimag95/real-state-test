using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealState.Test.Domain.Property;
using RealState.Test.Infrastructure.Persistence.Repositories;

namespace RealState.Test.Infrastructure.Persistence;

internal static class PersistenceServiceCollection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RealStateContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("database")));

        services.AddScoped<IPropertyRepository, PropertyRepository>();

        return services;
    }
}