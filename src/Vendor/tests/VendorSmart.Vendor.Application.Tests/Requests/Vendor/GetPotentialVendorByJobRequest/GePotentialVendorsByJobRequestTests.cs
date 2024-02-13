using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.SharedKernel.Boundaries.Job;
using VendorSmart.SharedKernel.Boundaries.Job.Models;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Application.Tests.Requests.Vendor.GetPotentialVendors;

public class GetPotentialVendorsByJobRequestHandlerTests
{
    private readonly IVendorRepository _vendorRepository = Substitute.For<IVendorRepository>();
    private readonly IJobBoundaryApi _jobBoundaryApi = Substitute.For<IJobBoundaryApi>();
    private readonly ILogger<GetPotentialVendorsByJobRequestHandler> _logger = Substitute.For<ILogger<GetPotentialVendorsByJobRequestHandler>>();
    private readonly GetPotentialVendorsByJobRequestHandler _handler;

    public GetPotentialVendorsByJobRequestHandlerTests()
    {
        _handler = new(_vendorRepository, _jobBoundaryApi, _logger);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldReturnAppException()
    {
        // Arrange
        var request = new GetPotentialVendorsByJobRequest(Guid.NewGuid());
        _jobBoundaryApi.When(x => x.GetByIdAsync(request.JobId))
            .Do(_ => throw new Exception("error"));

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<AppException>().Which.Type.Should().Be("GetPotentialVendorsByJobFailed");
        await _vendorRepository.DidNotReceiveWithAnyArgs().GetPotentialVendors(default!, default!, default!);
    }

    [Fact]
    public async Task Handle_WhenFailToGetJob_ReturnsException()
    {
        // Arrange
        var jobId = Guid.NewGuid();
        _jobBoundaryApi.GetByIdAsync(jobId).Returns(new AppException("error", "error"));
        var request = new GetPotentialVendorsByJobRequest(jobId);

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<AppException>().Which.Type.Should().Be("error");
        await _vendorRepository.DidNotReceiveWithAnyArgs().GetPotentialVendors(default!, default!, default!);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsCorrectVendors()
    {
        // Arrange
        var serviceCategory = new ServiceCategoryEntity("Landscaping");
        var location = new LocationEntity("Glades", "FL");

        var job = new GetJobPublicModel(Guid.NewGuid(), location.County, location.State, serviceCategory.Name);
        _jobBoundaryApi.GetByIdAsync(job.Id).Returns(job);

        var vendor1 = new VendorEntity("Vendor1", location.Id);
        vendor1.AddServiceCategory(serviceCategory, true);

        var vendor2 = new VendorEntity("Vendor2", location.Id);
        vendor2.AddServiceCategory(serviceCategory, false);

        var vendors = new List<VendorEntity>() { vendor1, vendor2 };
        _vendorRepository.GetPotentialVendors(serviceCategory.Name, location.County, location.State).Returns(vendors);

        var request = new GetPotentialVendorsByJobRequest(job.Id);

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
    public async Task Handle_WhenNoVendorsFound_ReturnsEmpty()
    {
        // Arrange
        var serviceCategory = new ServiceCategoryEntity("Landscaping");
        var location = new LocationEntity("Glades", "FL");

        var job = new GetJobPublicModel(Guid.NewGuid(), location.County, location.State, serviceCategory.Name);
        _jobBoundaryApi.GetByIdAsync(job.Id).Returns(job);

        _vendorRepository.GetPotentialVendors(serviceCategory.Name, location.County, location.State).Returns([]);

        var request = new GetPotentialVendorsByJobRequest(job.Id);

        // Act
        var (success, potentialVendors) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeTrue();
        potentialVendors!.Vendors.Should().HaveCount(0);
        await _vendorRepository.Received(1).GetPotentialVendors(serviceCategory.Name, location.County, location.State);
    }
}
