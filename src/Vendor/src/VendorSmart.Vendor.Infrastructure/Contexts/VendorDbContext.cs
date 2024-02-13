using Microsoft.EntityFrameworkCore;
using VendorSmart.Shared.Core.Domain.Contracts;
using VendorSmart.Vendor.Domain.Entities;
using VendorSmart.Vendor.Infrastructure.EntityConfigurations;

namespace VendorSmart.Vendor.Infrastructure.Contexts;

public class VendorDbContext(DbContextOptions<VendorDbContext> options) : DbContext(options), IDbContext
{
    public DbSet<VendorEntity> Vendors { get; set; }

    public Task SaveChangesAsync() => base.SaveChangesAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendorEntityConfiguration).Assembly);
    }
}