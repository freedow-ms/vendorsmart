using VendorSmart.Shared.Core.Domain;
using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.Job.Entities;

public class JobEntity : BaseEntity
{
    public string Name { get; private set; } = default!;
    public Guid ServiceCategoryId { get; private set; } = default!;

    public ServiceCategoryEntity ServiceCategory { get; private set; } = default!;

    public Guid LocationId { get; private set; } = default!;

    public LocationEntity Location { get; private set; } = default!;

    public JobEntity(string name, Guid locationId, Guid serviceCategoryId)
    {
        Name = name;
        LocationId = locationId;
        ServiceCategoryId = serviceCategoryId;
    }

    private JobEntity()
    {

    }
}
