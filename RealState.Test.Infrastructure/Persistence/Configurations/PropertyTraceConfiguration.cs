using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Test.Domain.Property;

namespace RealState.Test.Infrastructure.Persistence.Configurations;

public class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
{
    public void Configure(EntityTypeBuilder<PropertyTrace> builder)
    {
        builder.ToTable("PropertyTrace");
        
        builder.HasKey(x => x.IdPropertyTrace);
        
        builder.Property(x => x.IdPropertyTrace)
            .IsRequired();
        
        builder.Property(x => x.DateSale)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Value)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(x => x.Tax)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}