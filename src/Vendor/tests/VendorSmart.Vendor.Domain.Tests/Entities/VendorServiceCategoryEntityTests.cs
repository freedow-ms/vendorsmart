using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Domain.Tests.Entities;

public class VendorServiceCategoryEntityTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var vendorId = Guid.NewGuid();
        var serviceCategory = new ServiceCategoryEntity("Test Category");
        var isCompliant = true;

        // Act
        var entity = new VendorServiceCategoryEntity(vendorId, serviceCategory.Id, isCompliant);

        // Assert
        entity.VendorId.Should().Be(vendorId);
        entity.ServiceCategoryId.Should().Be(serviceCategory.Id);
        entity.IsCompliant.Should().Be(isCompliant);
    }

    [Fact]
    public void SetCompliance_ShouldUpdateIsCompliantProperty()
    {
        // Arrange
        var vendorId = Guid.NewGuid();
        var serviceCategory = new ServiceCategoryEntity("Test Category");
        var entity = new VendorServiceCategoryEntity(vendorId, serviceCategory.Id, false);

        // Act
        entity.SetCompliance(true);

        // Assert
        entity.IsCompliant.Should().BeTrue();
    }
}