using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Domain.Tests.Entities
{
    public class VendorEntityTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var name = "Name";
            var location = Guid.NewGuid();

            // Act
            var entity = new VendorEntity(name, location);

            // Assert
            entity.Name.Should().Be(name);
            entity.LocationId.Should().Be(location);
        }

        [Fact]
        public void AddServiceCategory_WhenNew_ShouldAddServiceCategory()
        {
            // Arrange
            var vendor = new VendorEntity("Vendor 1", Guid.NewGuid());
            var serviceCategory = new ServiceCategoryEntity("serviceCategory");

            // Act
            vendor.AddServiceCategory(serviceCategory.Id, false);

            // Assert
            vendor.ServiceCategories.Should().ContainSingle();
            vendor.ServiceCategories.First().IsCompliant.Should().BeFalse();
            vendor.ServiceCategories.First().ServiceCategoryId.Should().Be(serviceCategory.Id);
        }

        [Fact]
        public void AddServiceCategory_WhenAlreadyExists_ShouldNotAddServiceCategory()
        {
            // Arrange
            var vendor = new VendorEntity("Vendor 1", Guid.NewGuid());
            var serviceCategory = new ServiceCategoryEntity("serviceCategory");

            // Act
            vendor.AddServiceCategory(serviceCategory.Id, false);
            vendor.AddServiceCategory(serviceCategory.Id, true);

            // Assert
            vendor.ServiceCategories.Should().ContainSingle();
        }

        [Fact]
        public void SetCompliance_WhenServiceCategoryNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var vendor = new VendorEntity("Vendor 1", Guid.NewGuid());
            var serviceCategoryId = Guid.NewGuid();

            // Act
            Action act = () => vendor.SetCompliance(serviceCategoryId, true);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void SetCompliance_WhenServiceCategoryExists_ShouldUpdateCompliance()
        {
            // Arrange
            var serviceCategory = new ServiceCategoryEntity("serviceCategory");
            var vendor = new VendorEntity("Vendor 1", Guid.NewGuid());
            vendor.AddServiceCategory(serviceCategory.Id, false);

            // Act
            vendor.SetCompliance(serviceCategory.Id, true);

            // Assert
            vendor.ServiceCategories.First().IsCompliant.Should().BeTrue();
        }

        [Fact]
        public void UpdateLocation_WhenValidLocationProvided_ShouldUpdateLocation()
        {
            // Arrange
            var vendor = new VendorEntity("Vendor 1", Guid.NewGuid());
            var newLocation = new LocationEntity("County", "State");

            // Act
            vendor.UpdateLocation(newLocation);

            // Assert
            vendor.Location.Should().Be(newLocation);
            vendor.LocationId.Should().Be(newLocation.Id);
        }
    }
}
