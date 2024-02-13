using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.Vendor.Domain.Entities;

public class VendorServiceCategoryEntity
{
    public Guid VendorId { get; } = default!;

    public Guid ServiceCategoryId { get; } = default!;

    public ServiceCategoryEntity ServiceCategory { get; } = default!;

    public bool IsCompliant { get; private set; }

    public void SetCompliance(bool isCompliant) => IsCompliant = isCompliant;

    public VendorServiceCategoryEntity(Guid vendorId, Guid serviceCategoryId, bool isCompliant)
    {
        VendorId = vendorId;
        ServiceCategoryId = serviceCategoryId;
        IsCompliant = isCompliant;
    }

    public VendorServiceCategoryEntity(Guid vendorId, ServiceCategoryEntity serviceCategory, bool isCompliant)
    {
        VendorId = vendorId;
        ServiceCategoryId = serviceCategory.Id;
        ServiceCategory = serviceCategory;
        IsCompliant = isCompliant;
    }

    private VendorServiceCategoryEntity() { }
}
