using Microsoft.EntityFrameworkCore;
using VendorSmart.Shared.Core.Util.Specification.Evaluator;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Domain.Entities;
using VendorSmart.Vendor.Domain.Specifications;
using VendorSmart.Vendor.Infrastructure.Contexts;

namespace VendorSmart.Identity.Infrastructure.Repositories;

public class VendorRepository(VendorDbContext context, ISpecificationEvaluator specificationEvaluator) : IVendorRepository
{
    public async Task AddAsync(VendorEntity vendor) => await context.Vendors.AddAsync(vendor);

    public Task SaveAsync() => context.SaveChangesAsync();

    public Task<List<VendorEntity>> GetPotentialVendors(string serviceCategory, string county, string state, CancellationToken cancellationToken = default)
    {
        var query = context.Vendors
            .Include(v => v.Location)
            .Include(v => v.ServiceCategories)
            .ThenInclude(sc => sc.ServiceCategory);
        var specification = new PotentialVendorsSpecification(serviceCategory, county, state);
        return specificationEvaluator.GetQuery(query, specification).ToListAsync(cancellationToken);
    }

    public Task<int> GetPotentialVendorsCount(string serviceCategory, string county, string state, bool? isCompliant = null, CancellationToken cancellationToken = default)
    {
        var query = context.Vendors
            .Include(v => v.Location)
            .Include(v => v.ServiceCategories)
            .ThenInclude(sc => sc.ServiceCategory);
        var specification = new PotentialVendorsSpecification(serviceCategory, county, state, isCompliant);
        return specificationEvaluator.GetQuery(query, specification).CountAsync(cancellationToken);
    }
}
