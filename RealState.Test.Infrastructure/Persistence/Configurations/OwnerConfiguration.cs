using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealState.Test.Domain.Property;

namespace RealState.Test.Infrastructure.Persistence.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.ToTable("Owner");
        
        builder.HasKey(x => x.IdOwner);
        
        builder.Property(x => x.IdOwner)
            .IsRequired();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(455);
            
        builder.Property(x => x.Photo)
            .IsRequired()
            .HasMaxLength(1000);
            
        builder.Property(x => x.Birthday)
            .IsRequired();
        
        builder.HasMany(x => x.Properties)
            .WithOne(p => p.Owner)
            .HasForeignKey(p => p.IdOwner);
    }
}