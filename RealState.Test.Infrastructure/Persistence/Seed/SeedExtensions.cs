using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using RealState.Test.Domain.Property;

namespace RealState.Test.Infrastructure.Persistence.Seed;

public static class SeedExtensions
{
    public static async Task SeedDatabase(this IApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<RealStateContext>();
        
        await EnsureDatabaseAsync(dbContext, cancellationToken);
        
        await RunMigrationAsync(dbContext, cancellationToken);
        
        await SeedData(dbContext, cancellationToken);
    }
    
    private static async Task EnsureDatabaseAsync(RealStateContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }
    
    private static async Task RunMigrationAsync(RealStateContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
    
    private static async Task SeedData(RealStateContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            if (await dbContext.Owners.AnyAsync(cancellationToken)) return;

            var owner = Owner.Create("jhon doe", "test street", string.Empty, new DateTime(1995, 12, 12));
            
            dbContext.Owners.Add(owner);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        });
    }
}