using FluentAssertions;
using VendorSmart.Job.Entities;

namespace VendorSmart.Job.Domain.Tests.Entities;

public class JobEntityTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var name = "Name";
        var locationId = Guid.NewGuid();
        var serviceCategoryId = Guid.NewGuid();

        // Act
        var jobEntity = new JobEntity(name, locationId, serviceCategoryId);

        // Assert
        jobEntity.Name.Should().Be(name);
        jobEntity.LocationId.Should().Be(locationId);
        jobEntity.ServiceCategoryId.Should().Be(serviceCategoryId);
    }
}
