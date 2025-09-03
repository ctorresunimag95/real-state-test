using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RealState.Test.Infrastructure.Persistence;

public class DatabaseDesignTimeDbContextFactory : IDesignTimeDbContextFactory<RealStateContext>
{
    public RealStateContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"DesignTimeDbContextFactory executed with args: {string.Join(", ", args)}");

        var builder = new DbContextOptionsBuilder<RealStateContext>();
        builder.UseSqlServer();

        return new RealStateContext(builder.Options);
    }
}