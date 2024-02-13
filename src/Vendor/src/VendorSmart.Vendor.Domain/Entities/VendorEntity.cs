using VendorSmart.Shared.Core.Domain;
using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.Vendor.Domain.Entities;

public class VendorEntity : BaseEntity
{
    public string Name { get; private set; } = default!;

    private readonly List<VendorServiceCategoryEntity> _serviceCategories = [];

    public IReadOnlyCollection<VendorServiceCategoryEntity> ServiceCategories => _serviceCategories.AsReadOnly();

    public LocationEntity Location { get; private set; } = default!;

    public Guid LocationId { get; private set; } = default!;

    public VendorEntity(string name, Guid locationId)
    {
        Name = name;
        LocationId = locationId;
    }

    public VendorEntity(string name, LocationEntity location)
    {
        Name = name;
        Location = location;
        LocationId = location.Id;
    }

    public void SetCompliance(Guid serviceCategoryId, bool complianceStatus)
    {
        var serviceCategory = _serviceCategories.FirstOrDefault(sc => sc.ServiceCategoryId == serviceCategoryId)
            ?? throw new InvalidOperationException();
        serviceCategory.SetCompliance(complianceStatus);
    }

    public void AddServiceCategory(Guid serviceCategoryId, bool isCompliant)
    {
        if (!_serviceCategories.Any(s => s.ServiceCategoryId == serviceCategoryId))
            _serviceCategories.Add(new(Id, serviceCategoryId, isCompliant));
    }

    public void AddServiceCategory(ServiceCategoryEntity serviceCategory, bool isCompliant)
    {
        if (!_serviceCategories.Any(s => s.ServiceCategoryId == serviceCategory.Id))
            _serviceCategories.Add(new(Id, serviceCategory, isCompliant));
    }

    public void UpdateLocation(LocationEntity location)
    {
        Location = location ?? throw new ArgumentNullException(nameof(location));
        LocationId = location.Id;
    }

    private VendorEntity()
    {
    }
}
