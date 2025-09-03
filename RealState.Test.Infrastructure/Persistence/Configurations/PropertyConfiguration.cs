using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Test.Domain.Property;

namespace RealState.Test.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Property");
        
        builder.HasKey(x => x.IdProperty);
        
        builder.Property(x => x.IdProperty)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Address)
            .HasMaxLength(455)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(x => x.CodeInternal)
            .IsRequired();
        
        builder.Property(x => x.Year)
            .IsRequired();

        builder.HasOne(x => x.Owner)
            .WithMany(x => x.Properties)
            .HasForeignKey(x => x.IdOwner);
        
        builder.HasMany(x => x.PropertyImages)
            .WithOne()
            .HasForeignKey(x => x.IdProperty);
        
        builder.HasMany(x => x.PropertyTraces)
            .WithOne()
            .HasForeignKey(x => x.IdProperty);
    }
}