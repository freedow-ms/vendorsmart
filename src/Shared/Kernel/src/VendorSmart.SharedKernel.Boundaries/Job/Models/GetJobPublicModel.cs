namespace VendorSmart.SharedKernel.Boundaries.Job.Models;

public record GetJobPublicModel(Guid Id, string County, string State, string ServiceCategory);