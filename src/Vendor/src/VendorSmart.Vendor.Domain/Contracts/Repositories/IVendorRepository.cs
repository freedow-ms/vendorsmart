using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Domain.Contracts.Repositories;

public interface IVendorRepository
{
    Task AddAsync(VendorEntity vendor);

    Task<List<VendorEntity>> GetPotentialVendors(string serviceCategory, string county, string state, CancellationToken cancellationToken = default);

    Task<int> GetPotentialVendorsCount(string serviceCategory, string county, string state, bool? isCompliant = null, CancellationToken cancellationToken = default);

    Task SaveAsync();
}
