using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Vendor.Application.Requests.Vendor.GetTotalVendors;
using VendorSmart.Vendor.Domain.Contracts.Repositories;

namespace VendorSmart.Vendor.Application.Tests.Requests.Vendor.GetTotalVendors;

public class GetTotalVendorsRequestTests
{
    private readonly IVendorRepository _vendorRepository = Substitute.For<IVendorRepository>();
    private readonly ILogger<GetTotalVendorsRequestHandler> _logger = Substitute.For<ILogger<GetTotalVendorsRequestHandler>>();
    private readonly GetTotalVendorsRequestHandler _handler;

    public GetTotalVendorsRequestTests()
    {
        _handler = new(_vendorRepository, _logger);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnTotalVendors()
    {
        // Arrange
        var serviceCategory = "Landscaping";
        var county = "Glades";
        var state = "FL";

        _vendorRepository.GetPotentialVendorsCount(serviceCategory, county, state, isCompliant: false)
            .Returns(2);
        _vendorRepository.GetPotentialVendorsCount(serviceCategory, county, state, isCompliant: true)
            .Returns(1);

        // Act
        var (success, vendors, _) = await _handler.Handle(new(serviceCategory, county, state), default!);

        // Assert
        success.Should().BeTrue();
        vendors!.Total.Should().Be(3);
        vendors!.Compliant.Should().Be(1);
        vendors!.NonCompliant.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldReturnAppException()
    {
        // Arrange
        var serviceCategory = "Landscaping";
        var county = "Glades";
        var state = "FL";
        _vendorRepository.When(x => x.GetPotentialVendorsCount(serviceCategory, county, state, isCompliant: true))
            .Do(_ => throw new Exception("error"));

        // Act
        var (success, _, exception) = await _handler.Handle(new(serviceCategory, county, state), default!);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<AppException>().Which.Type.Should().Be("GetTotalVendorsFailed");
    }


    [Fact]
    public async Task Handle_WhenNoVendorsFound_ShouldReturnTotalVendors()
    {
        // Arrange
        var serviceCategory = "Landscaping";
        var county = "Glades";
        var state = "FL";

        _vendorRepository.GetPotentialVendorsCount(serviceCategory, county, state, isCompliant: false)
            .Returns(0);
        _vendorRepository.GetPotentialVendorsCount(serviceCategory, county, state, isCompliant: true)
            .Returns(0);

        // Act
        var (success, vendors, _) = await _handler.Handle(new(serviceCategory, county, state), default!);

        // Assert
        success.Should().BeTrue();
        vendors!.Total.Should().Be(0);
        vendors!.Compliant.Should().Be(0);
        vendors!.NonCompliant.Should().Be(0);
    }
}

