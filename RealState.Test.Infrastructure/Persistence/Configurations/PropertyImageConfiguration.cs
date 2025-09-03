using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Test.Domain.Property;

namespace RealState.Test.Infrastructure.Persistence.Configurations;

public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
{
    public void Configure(EntityTypeBuilder<PropertyImage> builder)
    {
        builder.ToTable("PropertyImage");
        
        builder.HasKey(x => x.IdPropertyImage);
        
        builder.Property(x => x.IdPropertyImage)
            .IsRequired();
        
        builder.Property(x => x.File)
            .HasMaxLength(1000)
            .IsRequired();
        
        builder.Property(x => x.Enabled)
            .IsRequired();
    }
}