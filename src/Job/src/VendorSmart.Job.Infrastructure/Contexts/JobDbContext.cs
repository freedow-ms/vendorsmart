using Microsoft.EntityFrameworkCore;
using VendorSmart.Job.Entities;
using VendorSmart.Shared.Core.Domain.Contracts;

namespace VendorSmart.Job.Infrastructure.Contexts;

public class JobDbContext(DbContextOptions<JobDbContext> options) : DbContext(options), IDbContext
{
    public DbSet<JobEntity> Jobs { get; set; }

    public Task SaveChangesAsync() => base.SaveChangesAsync();
}