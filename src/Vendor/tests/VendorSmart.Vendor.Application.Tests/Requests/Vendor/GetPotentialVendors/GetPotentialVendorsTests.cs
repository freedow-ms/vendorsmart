using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Application.Tests.Requests.Vendor.GetPotentialVendors;

public class GePotentialVendorsRequestHandlerTests
{
    private readonly IVendorRepository _vendorRepository = Substitute.For<IVendorRepository>();
    private readonly ILogger<GetPotentialVendorsRequestHandler> _logger = Substitute.For<ILogger<GetPotentialVendorsRequestHandler>>();
    private readonly GetPotentialVendorsRequestHandler _handler;

    public GePotentialVendorsRequestHandlerTests()
    {
        _handler = new(_vendorRepository, _logger);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsCorrectVendors()
    {
        // Arrange
        var serviceCategory = new ServiceCategoryEntity("Landscaping");
        var location = new LocationEntity("Glades", "FL");
        var request = new GetPotentialVendorsRequest(serviceCategory.Name, location.County, location.State);

        var vendor1 = new VendorEntity("Vendor1", location.Id);
        vendor1.AddServiceCategory(serviceCategory, true);

        var vendor2 = new VendorEntity("Vendor2", location.Id);
        vendor2.AddServiceCategory(serviceCategory, false);

        var vendors = new List<VendorEntity>() { vendor1, vendor2 };
        _vendorRepository.GetPotentialVendors(serviceCategory.Name, location.County, location.State).Returns(vendors);

        // Act
        var (success, potentialVendors) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeTrue();
        potentialVendors!.Vendors.Should().HaveCount(vendors.Count);
        potentialVendors!.Vendors.First().IsCompliant.Should().BeTrue();
        potentialVendors!.Vendors.Last().IsCompliant.Should().BeFalse();
        await _vendorRepository.Received(1).GetPotentialVendors(serviceCategory.Name, location.County, location.State);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldReturnAppException()
    {
        // Arrange
        var serviceCategory = new ServiceCategoryEntity("Landscaping");
        var location = new LocationEntity("Glades", "FL");
        var request = new GetPotentialVendorsRequest(serviceCategory.Name, location.County, location.State);
        _vendorRepository.When(x => x.GetPotentialVendors(serviceCategory.Name, location.County, location.State))
            .Do(_ => throw new Exception("error"));

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<AppException>().Which.Type.Should().Be("GetPotentialVendorsFailed");
    }

    [Fact]
    public async Task Handle_WhenNoVendorsFound_ReturnsEmpty()
    {
        // Arrange
        var serviceCategory = new ServiceCategoryEntity("Landscaping");
        var location = new LocationEntity("Glades", "FL");
        var request = new GetPotentialVendorsRequest(serviceCategory.Name, location.County, location.State);

        _vendorRepository.GetPotentialVendors(serviceCategory.Name, location.County, location.State).Returns([]);

        // Act
        var (success, potentialVendors) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeTrue();
        potentialVendors!.Vendors.Should().HaveCount(0);
        await _vendorRepository.Received(1).GetPotentialVendors(serviceCategory.Name, location.County, location.State);
    }
}
