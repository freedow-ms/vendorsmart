using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Infrastructure.EntityConfigurations
{
    public class VendorServiceCategoryConfiguration : IEntityTypeConfiguration<VendorServiceCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<VendorServiceCategoryEntity> builder)
        {
            builder.ToTable("VendorServiceCategories");

            builder.HasKey(vsc => new { vsc.VendorId, vsc.ServiceCategoryId });

            builder.HasOne(vsc => vsc.ServiceCategory)
                   .WithMany()
                   .HasForeignKey(vsc => vsc.ServiceCategoryId)
                   .IsRequired();

            builder.Property(vsc => vsc.IsCompliant).IsRequired();
        }
    }
}
