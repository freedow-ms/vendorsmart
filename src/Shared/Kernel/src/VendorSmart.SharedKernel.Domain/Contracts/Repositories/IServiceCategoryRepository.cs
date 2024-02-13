using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.SharedKernel.Domain.Contracts.Repositories;

public interface IServiceCategoryRepository
{
    Task<ServiceCategoryEntity?> GetByIdAsync(Guid serviceCategoryId);

    Task<ServiceCategoryEntity?> GetByNameAsync(string name);

    Task AddAsync(ServiceCategoryEntity location);
}
