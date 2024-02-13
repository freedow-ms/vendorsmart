using VendorSmart.Shared.Core.Util.Specification.Evaluator;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.Vendor.Domain.Entities;
using VendorSmart.Vendor.Domain.Specifications;

namespace VendorSmart.Vendor.Domain.Tests.Specifications;
public class PotentialVendorSpecificationTests
{

    [Fact]
    public void PotentialVendorsSpecification_WhenMultipleVendorsForJob_ShouldReturnCorrectVendorsOrderByCompliant()
    {
        // Arrange
        var glades = new LocationEntity("Glades", "FL");

        var landscaping = new ServiceCategoryEntity("Landscaping");

        var vendor1 = new VendorEntity("Compliant Vendor 1", glades);
        vendor1.AddServiceCategory(landscaping, true);

        var vendor2 = new VendorEntity("NonCompliant Vendor 2", glades);
        vendor2.AddServiceCategory(landscaping, false);

        var vendor3 = new VendorEntity("Compliant Vendor 3", glades);
        vendor3.AddServiceCategory(landscaping, true);

        var vendors = new List<VendorEntity>() { vendor1, vendor2, vendor3 }.AsQueryable();
        var specification = new PotentialVendorsSpecification(landscaping.Name, glades.County, glades.State);

        // Act
        var potentialVendors = new SpecificationEvaluator().GetQuery(vendors, specification).ToList();

        // Assert
        potentialVendors.Should().HaveCount(3);
        potentialVendors[0].ServiceCategories.First().IsCompliant.Should().BeTrue();
        potentialVendors[1].ServiceCategories.First().IsCompliant.Should().BeTrue();
        potentialVendors[2].ServiceCategories.First().IsCompliant.Should().BeFalse();
        potentialVendors.Should().AllSatisfy(vendor =>
        {
            vendor.Location.County.Should().Be(glades.County);
            vendor.Location.State.Should().Be(glades.State);
        });
        potentialVendors.Should().AllSatisfy(v => v.ServiceCategories.Any(sc => sc.ServiceCategory.Name == landscaping.Name));
    }

    [Fact]
    public void PotentialVendorsSpecification_WhenVendorsWithOtherServiceCaregories_ShouldReturnOnlyVendorsWithCorrectCategory()
    {
        // Arrange
        var glades = new LocationEntity("Glades", "FL");
        var landscaping = new ServiceCategoryEntity("Landscaping");
        var airConditioning = new ServiceCategoryEntity("Air Conditioning");

        var vendor1 = new VendorEntity("Compliant Vendor 1", glades);
        vendor1.AddServiceCategory(airConditioning, true);

        var vendor2 = new VendorEntity("NonCompliant Vendor 2", glades);
        vendor2.AddServiceCategory(landscaping, false);

        var vendor3 = new VendorEntity("Compliant Vendor 3", glades);
        vendor3.AddServiceCategory(airConditioning, true);

        var vendors = new List<VendorEntity>() { vendor1, vendor2, vendor3 }.AsQueryable();
        var specification = new PotentialVendorsSpecification(landscaping.Name, glades.County, glades.State);

        // Act
        var potentialVendors = new SpecificationEvaluator().GetQuery(vendors, specification).ToList();


        // Assert
        potentialVendors.Should().HaveCount(1);
        potentialVendors.First().Name.Should().Be(vendor2.Name);
        potentialVendors.First().Location.County.Should().Be(glades.County);
        potentialVendors.First().Location.State.Should().Be(glades.State);
        potentialVendors.Should().AllSatisfy(v => v.ServiceCategories.Any(sc => sc.ServiceCategory.Name == landscaping.Name));
    }

    [Fact]
    public void PotentialVendorsSpecification_WhenVendorsInOtherLocations_ShouldReturnOnlyVendorsInSameLocation()
    {
        // Arrange
        var glades = new LocationEntity("Glades", "FL");
        var hamilton = new LocationEntity("Hamilton", "FL");
        var landscaping = new ServiceCategoryEntity("Landscaping");

        var vendor1 = new VendorEntity("Compliant Vendor 1", hamilton);
        vendor1.AddServiceCategory(landscaping, true);

        var vendor2 = new VendorEntity("NonCompliant Vendor 2", glades);
        vendor2.AddServiceCategory(landscaping, false);


        var vendor3 = new VendorEntity("Compliant Vendor 3", hamilton);
        vendor3.AddServiceCategory(landscaping, true);

        var vendors = new List<VendorEntity>() { vendor1, vendor2, vendor3 }.AsQueryable();
        var specification = new PotentialVendorsSpecification(landscaping.Name, glades.County, glades.State);

        // Act
        var potentialVendors = new SpecificationEvaluator().GetQuery(vendors, specification).ToList();


        // Assert
        potentialVendors.Should().HaveCount(1);
        potentialVendors.First().Name.Should().Be(vendor2.Name);
        potentialVendors.First().Location.County.Should().Be(glades.County);
        potentialVendors.First().Location.State.Should().Be(glades.State);
        potentialVendors.Should().AllSatisfy(v => v.ServiceCategories.Any(sc => sc.ServiceCategory.Name == landscaping.Name));
    }
}
