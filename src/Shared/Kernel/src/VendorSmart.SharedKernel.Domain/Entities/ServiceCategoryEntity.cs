using VendorSmart.Shared.Core.Domain;

namespace VendorSmart.SharedKernel.Domain.Entities;

public class ServiceCategoryEntity : BaseEntity
{
    public string Name { get; private set; } = default!;

    public ServiceCategoryEntity(string name) => Name = name;
}
