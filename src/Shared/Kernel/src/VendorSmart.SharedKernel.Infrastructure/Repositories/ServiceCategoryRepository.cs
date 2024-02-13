using Microsoft.EntityFrameworkCore;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.SharedKernel.Infrastructure.Contexts;

namespace VendorSmart.SharedKernel.Infrastructure.Repositories;

public class ServiceCategoryRepository(SharedDbContext context) : IServiceCategoryRepository
{
    public Task<ServiceCategoryEntity?> GetByIdAsync(Guid serviceCategoryId) => context.ServiceCategories.FirstOrDefaultAsync(x => x.Id == serviceCategoryId);

    public Task<ServiceCategoryEntity?> GetByNameAsync(string name) => context.ServiceCategories.FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

    public async Task AddAsync(ServiceCategoryEntity serviceCategory) => await context.ServiceCategories.AddAsync(serviceCategory);
}
