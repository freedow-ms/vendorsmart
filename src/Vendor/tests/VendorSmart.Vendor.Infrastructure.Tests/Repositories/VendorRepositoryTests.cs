using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using VendorSmart.Identity.Infrastructure.Repositories;
using VendorSmart.Shared.Core.Util.Specification;
using VendorSmart.Shared.Core.Util.Specification.Evaluator;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.SharedKernel.Infrastructure.Contexts;
using VendorSmart.Vendor.Domain.Entities;
using VendorSmart.Vendor.Domain.Specifications;
using VendorSmart.Vendor.Infrastructure.Contexts;

namespace VendorSmart.Vendor.Infrastructure.Tests.Repositories;

public class VendorRepositoryTests
{
    private readonly DbContextOptions<VendorDbContext> _dbContextOptions;
    private readonly DbContextOptions<SharedDbContext> _sharedDbContextOptions;
    private readonly ISpecificationEvaluator _specificationEvaluator = Substitute.For<ISpecificationEvaluator>();

    public VendorRepositoryTests()
    {
        // Unique name for each test run
        var dbName = $"VendorDb_{DateTime.Now.ToFileTimeUtc()}";
        _sharedDbContextOptions = new DbContextOptionsBuilder<SharedDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        _dbContextOptions = new DbContextOptionsBuilder<VendorDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
    }

    [Fact]
    public async Task AddAsync_AddsVendorSuccessfully()
    {
        // Arrange
        using var context = new VendorDbContext(_dbContextOptions);
        var repository = new VendorRepository(context, _specificationEvaluator);
        var newVendor = new VendorEntity("New Vendor", Guid.NewGuid());

        // Act
        await repository.AddAsync(newVendor);
        await context.SaveChangesAsync();

        // Assert
        var vendorInDb = await context.Vendors.FindAsync(newVendor.Id);
        vendorInDb.Should().NotBeNull();
        vendorInDb!.Name.Should().Be("New Vendor");
    }

    [Fact]
    public async Task GetPotentialVendors_ShouldReturnCorrectVendorsOrderByCompliant()
    {
        // Arrange
        var (vendorContext, sharedContext) = CreateDatabases();

        var glades = new LocationEntity("Glades", "FL");
        sharedContext.Locations.Add(glades);

        var landscaping = new ServiceCategoryEntity("Landscaping");
        sharedContext.ServiceCategories.Add(landscaping);

        sharedContext.SaveChanges();

        var vendor1 = new VendorEntity("Compliant Vendor 1", glades.Id);
        vendor1.AddServiceCategory(landscaping.Id, true);
        vendorContext.Vendors.Add(vendor1);

        var vendor2 = new VendorEntity("NonCompliant Vendor 2", glades.Id);
        vendor2.AddServiceCategory(landscaping.Id, false);
        vendorContext.Vendors.Add(vendor2);


        var vendor3 = new VendorEntity("Compliant Vendor 3", glades.Id);
        vendor3.AddServiceCategory(landscaping.Id, true);
        vendorContext.Vendors.Add(vendor3);

        vendorContext.SaveChanges();
        var specification = new PotentialVendorsSpecification(landscaping.Name, glades.County, glades.State);

        _specificationEvaluator.GetQuery(Arg.Any<IQueryable<VendorEntity>>(), Arg.Is<ISpecification<VendorEntity>>(spec => spec.GetType() == typeof(PotentialVendorsSpecification)))
            .Returns(info => new SpecificationEvaluator().GetQuery((IQueryable<VendorEntity>)info[0], specification));

        // Act
        var repository = new VendorRepository(vendorContext, _specificationEvaluator);
        var vendors = await repository.GetPotentialVendors(landscaping.Name, glades.County, glades.State);


        // Assert
        vendors.Should().HaveCount(3);
        vendors[0].ServiceCategories.First().IsCompliant.Should().BeTrue();
        vendors[1].ServiceCategories.First().IsCompliant.Should().BeTrue();
        vendors[2].ServiceCategories.First().IsCompliant.Should().BeFalse();
        vendors.Should().AllSatisfy(vendor =>
        {
            vendor.Location.County.Should().Be(glades.County);
            vendor.Location.State.Should().Be(glades.State);
        });
        vendors.Should().AllSatisfy(v => v.ServiceCategories.Any(sc => sc.ServiceCategory.Name == landscaping.Name));
    }

    [Fact]
    public async Task GetPotentialVendorsCount_ShouldReturnTotalVendors()
    {
        // Arrange
        var (vendorContext, sharedContext) = CreateDatabases();

        var glades = new LocationEntity("Glades", "FL");
        sharedContext.Locations.Add(glades);

        var gulf = new LocationEntity("Gulf", "FL");
        sharedContext.Locations.Add(glades);

        var landscaping = new ServiceCategoryEntity("Landscaping");
        sharedContext.ServiceCategories.Add(landscaping);

        sharedContext.SaveChanges();

        var vendor1 = new VendorEntity("Compliant Vendor 1", glades.Id);
        vendor1.AddServiceCategory(landscaping.Id, true);
        vendorContext.Vendors.Add(vendor1);

        var vendor2 = new VendorEntity("NonCompliant Vendor 2", glades.Id);
        vendor2.AddServiceCategory(landscaping.Id, false);
        vendorContext.Vendors.Add(vendor2);

        var vendor3 = new VendorEntity("Compliant Vendor 3", gulf.Id);
        vendor3.AddServiceCategory(landscaping.Id, true);
        vendorContext.Vendors.Add(vendor3);

        vendorContext.SaveChanges();
        var specification = new PotentialVendorsSpecification(landscaping.Name, glades.County, glades.State);

        _specificationEvaluator.GetQuery(Arg.Any<IQueryable<VendorEntity>>(), Arg.Is<ISpecification<VendorEntity>>(spec => spec.GetType() == typeof(PotentialVendorsSpecification)))
            .Returns(info => new SpecificationEvaluator().GetQuery((IQueryable<VendorEntity>)info[0], specification));

        // Act
        var repository = new VendorRepository(vendorContext, _specificationEvaluator);
        var count = await repository.GetPotentialVendorsCount(landscaping.Name, glades.County, glades.State);


        // Assert
        count.Should().Be(2);
    }

    private (VendorDbContext vendorContext, SharedDbContext sharedContext) CreateDatabases()
    {
        VendorDbContext vendorContext = new(_dbContextOptions);
        vendorContext.Database.EnsureDeleted();
        vendorContext.Database.EnsureCreated();

        SharedDbContext sharedContext = new(_sharedDbContextOptions);
        sharedContext.Database.EnsureDeleted();
        sharedContext.Database.EnsureCreated();

        return (vendorContext, sharedContext);
    }
}