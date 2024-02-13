using Microsoft.EntityFrameworkCore;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.SharedKernel.Infrastructure.Contexts;

namespace VendorSmart.SharedKernel.Infrastructure.Repositories;

public class LocationRepository(SharedDbContext context) : ILocationRepository
{
    public Task<LocationEntity?> GetByIdAsync(Guid locationId) => context.Locations.FirstOrDefaultAsync(x => x.Id == locationId);

    public Task<LocationEntity?> GetAsync(string county, string state) => context.Locations.FirstOrDefaultAsync(x => x.County.Equals(county, StringComparison.InvariantCultureIgnoreCase) && x.State.Equals(state, StringComparison.InvariantCultureIgnoreCase));

    public async Task AddAsync(LocationEntity location) => await context.Locations.AddAsync(location);
}
