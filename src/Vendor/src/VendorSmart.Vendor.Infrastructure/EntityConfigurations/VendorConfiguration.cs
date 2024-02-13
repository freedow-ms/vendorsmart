using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Infrastructure.EntityConfigurations;


public class VendorEntityConfiguration : IEntityTypeConfiguration<VendorEntity>
{
    public void Configure(EntityTypeBuilder<VendorEntity> builder)
    {
        builder.ToTable("Vendors");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(v => v.Location)
            .WithMany()
            .HasForeignKey(v => v.LocationId)
            .IsRequired();

        builder.HasMany(v => v.ServiceCategories)
            .WithOne()
            .HasForeignKey(sc => sc.VendorId);

        builder.Metadata.FindNavigation(nameof(VendorEntity.ServiceCategories))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}