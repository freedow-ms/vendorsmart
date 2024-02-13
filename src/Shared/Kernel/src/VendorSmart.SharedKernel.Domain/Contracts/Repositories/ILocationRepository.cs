using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.SharedKernel.Domain.Contracts.Repositories;

public interface ILocationRepository
{
    Task<LocationEntity?> GetByIdAsync(Guid locationId);

    Task<LocationEntity?> GetAsync(string name, string state);

    Task AddAsync(LocationEntity location);
}
