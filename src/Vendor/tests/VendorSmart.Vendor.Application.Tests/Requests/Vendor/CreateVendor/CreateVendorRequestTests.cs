using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VendorSmart.Shared.Core.Domain.Exceptions;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.Vendor.Application.Requests.Vendor.CreateVendor;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Application.Tests.Requests.Vendor.CreateVendor;

public class CreateVendorRequestTests
{
    private readonly IRequestHandler<CreateVendorRequest, Result<CreateVendorResult>> _handler;
    private readonly IVendorRepository _vendorRepository = Substitute.For<IVendorRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();
    private readonly IServiceCategoryRepository _serviceCategoryRepository = Substitute.For<IServiceCategoryRepository>();
    private readonly ILogger<CreateVendorRequestHandler> _logger = Substitute.For<ILogger<CreateVendorRequestHandler>>();

    public CreateVendorRequestTests()
    {
        _handler = new CreateVendorRequestHandler(_vendorRepository, _locationRepository, _serviceCategoryRepository, _logger);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldReturnAppException()
    {
        // Arrange
        var request = new CreateVendorRequest("VendorName", [], new CreateVendorRequestLocation("county", "state"));
        _locationRepository.When(x => x.GetAsync("county", "state"))
            .Do(_ => throw new Exception("error"));

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<AppException>().Which.Type.Should().Be("CreateVendorFailed");
        await _vendorRepository.DidNotReceiveWithAnyArgs().AddAsync(default!);
        await _vendorRepository.DidNotReceiveWithAnyArgs().SaveAsync();
    }

    [Fact]
    public async Task Handle_WhenInvalidLocation_ShouldReturnBusinessException()
    {
        // Arrange
        var request = new CreateVendorRequest("VendorName", [], new CreateVendorRequestLocation("InvalidCounty", "InvalidState"));
        _locationRepository.GetAsync("InvalidCounty", "InvalidState").Returns((LocationEntity?)null);

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<BusinessException>().Which.Type.Should().Be("InvalidLocation");
        await _vendorRepository.DidNotReceiveWithAnyArgs().AddAsync(default!);
        await _vendorRepository.DidNotReceiveWithAnyArgs().SaveAsync();
    }

    [Fact]
    public async Task Handle_WhenInvalidServiceCategory_ShouldReturnBusinessException()
    {
        // Arrange
        var request = new CreateVendorRequest(
            "VendorName",
            new List<CreateVendorRequestServiceCategory>
            {
                new CreateVendorRequestServiceCategory("InvalidServiceCategory", true)
            },
            new CreateVendorRequestLocation("County", "State"));

        var expectedLocation = new LocationEntity("County", "State");
        _locationRepository.GetAsync("County", "State").Returns(expectedLocation);
        _serviceCategoryRepository.GetByNameAsync("InvalidServiceCategory").Returns((ServiceCategoryEntity?)null);

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<BusinessException>().Which.Type.Should().Be("InvalidServiceCategory");
        await _vendorRepository.DidNotReceiveWithAnyArgs().AddAsync(default!);
        await _vendorRepository.DidNotReceiveWithAnyArgs().SaveAsync();
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreateVendor()
    {
        // Arrange
        var request = new CreateVendorRequest(
            "VendorName",
            [
                new CreateVendorRequestServiceCategory("ServiceCategory1", true),
                new CreateVendorRequestServiceCategory("ServiceCategory2", false),
            ],
            new CreateVendorRequestLocation("County", "State"));

        var expectedLocation = new LocationEntity("County", "State");
        _locationRepository.GetAsync("County", "State").Returns(expectedLocation);

        var serviceCategory1 = new ServiceCategoryEntity("ServiceCategory1");
        _serviceCategoryRepository.GetByNameAsync("ServiceCategory1").Returns(serviceCategory1);

        var serviceCategory2 = new ServiceCategoryEntity("ServiceCategory2");
        _serviceCategoryRepository.GetByNameAsync("ServiceCategory2").Returns(serviceCategory2);

        // Act
        var (success, result) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeTrue();
        result!.Name.Should().Be(request.Name);
        result.Location.County.Should().Be(expectedLocation.County);
        result.Location.State.Should().Be(expectedLocation.State);
        result.Services.Should().HaveCount(2);
        result.Services.ToList()[0].Name.Should().Be(serviceCategory1.Name);
        result.Services.ToList()[0].Compliant.Should().Be(true);
        result.Services.ToList()[1].Name.Should().Be(serviceCategory2.Name);
        result.Services.ToList()[1].Compliant.Should().Be(false);

        await _vendorRepository.Received(1).AddAsync(
            Arg.Is<VendorEntity>(v =>
                v.Name == request.Name &&
                v.ServiceCategories.Count == 2 &&
                v.ServiceCategories.ToList()[0].ServiceCategoryId == serviceCategory1.Id &&
                v.ServiceCategories.ToList()[0].IsCompliant == true &&
                v.ServiceCategories.ToList()[1].ServiceCategoryId == serviceCategory2.Id &&
                v.ServiceCategories.ToList()[1].IsCompliant == false &&
                v.LocationId == expectedLocation.Id)
        );
        await _vendorRepository.Received(1).SaveAsync();
    }
}
