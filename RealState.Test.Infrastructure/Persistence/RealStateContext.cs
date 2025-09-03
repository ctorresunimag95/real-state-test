using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RealState.Test.Domain.Property;
using RealState.Test.Infrastructure.Persistence.Configurations;

namespace RealState.Test.Infrastructure.Persistence;

public class RealStateContext : DbContext
{
    public DbSet<Property> Properties { get; set; }
    
    public DbSet<Owner> Owners { get; set; }
    
    public DbSet<PropertyImage> PropertyImages { get; set; }
    
    public DbSet<PropertyTrace> PropertyTraces { get; set; }
    
    public RealStateContext(DbContextOptions<RealStateContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropertyConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder); 
    }
}