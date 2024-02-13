using Microsoft.EntityFrameworkCore;
using VendorSmart.Shared.Core.Domain.Contracts;
using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.SharedKernel.Infrastructure.Contexts;

public class SharedDbContext(DbContextOptions<SharedDbContext> options) : DbContext(options), IDbContext
{
    public DbSet<LocationEntity> Locations { get; set; }

    public DbSet<ServiceCategoryEntity> ServiceCategories { get; set; }

    Task IDbContext.SaveChangesAsync() => base.SaveChangesAsync();
}