using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VendorSmart.Job.Application.Requests.Job.CreateJob;
using VendorSmart.Job.Contracts.Repositories;
using VendorSmart.Job.Entities;
using VendorSmart.Shared.Core.Domain.Exceptions;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.Job.Application.Tests.Requests.Job;

public class CreateJobRequestTests
{
    private readonly IRequestHandler<CreateJobRequest, Result<CreateJobResult>> _handler;
    private readonly IJobRepository _jobRepository = Substitute.For<IJobRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();
    private readonly IServiceCategoryRepository _serviceCategoryRepository = Substitute.For<IServiceCategoryRepository>();
    private readonly ILogger<CreateJobRequestHandler> _logger = Substitute.For<ILogger<CreateJobRequestHandler>>();

    public CreateJobRequestTests()
    {
        _handler = new CreateJobRequestHandler(_jobRepository, _locationRepository, _serviceCategoryRepository, _logger);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreateJob()
    {
        // Arrange
        var request = new CreateJobRequest(
            "JobName",
            "ServiceCategory",
            new CreateJobRequestLocation("County", "State"));

        var expectedLocation = new LocationEntity("County", "State");
        var expectedServiceCategory = new ServiceCategoryEntity("ServiceCategory");

        _locationRepository.GetAsync("County", "State").Returns(expectedLocation);
        _serviceCategoryRepository.GetByNameAsync("ServiceCategory").Returns(expectedServiceCategory);

        // Act
        var (success, job) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeTrue();
        job!.Name.Should().Be(request.Name);
        job.Location.County.Should().Be(expectedLocation.County);
        job.Location.State.Should().Be(expectedLocation.State);
        job.ServiceCategory.Should().Be(expectedServiceCategory.Name);

        await _jobRepository.Received(1).AddAsync(Arg.Any<JobEntity>());
        await _jobRepository.Received(1).SaveAsync();
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldReturnAppException()
    {
        // Arrange
        var request = new CreateJobRequest("JobName", "ServiceCategory", new CreateJobRequestLocation("County", "State"));
        _locationRepository.When(x => x.GetAsync("County", "State"))
            .Do(_ => throw new Exception("error"));

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<AppException>().Which.Type.Should().Be("CreateJobFailed");

        await _jobRepository.DidNotReceiveWithAnyArgs().AddAsync(default!);
    }

    [Fact]
    public async Task Handle_WhenInvalidLocation_ShouldReturnBusinessException()
    {
        // Arrange
        var request = new CreateJobRequest("JobName", "ServiceCategory", new CreateJobRequestLocation("InvalidCounty", "InvalidState"));
        _locationRepository.GetAsync("InvalidCounty", "InvalidState").Returns((LocationEntity?)null);

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<BusinessException>().Which.Type.Should().Be("InvalidLocation");

        await _jobRepository.DidNotReceiveWithAnyArgs().AddAsync(default!);
    }

    [Fact]
    public async Task Handle_WhenInvalidServiceCategory_ShouldReturnBusinessException()
    {
        // Arrange
        var request = new CreateJobRequest("JobName", "InvalidServiceCategory", new CreateJobRequestLocation("County", "State"));
        _locationRepository.GetAsync("County", "State").Returns(new LocationEntity("County", "State"));
        _serviceCategoryRepository.GetByNameAsync("InvalidServiceCategory").Returns((ServiceCategoryEntity?)null);

        // Act
        var (success, _, exception) = await _handler.Handle(request, CancellationToken.None);

        // Assert
        success.Should().BeFalse();
        exception.Should().BeOfType<BusinessException>().Which.Type.Should().Be("InvalidServiceCategory");

        await _jobRepository.DidNotReceiveWithAnyArgs().AddAsync(default!);
    }
}
