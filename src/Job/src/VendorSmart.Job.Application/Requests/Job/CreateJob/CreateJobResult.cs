namespace VendorSmart.Job.Application.Requests.Job.CreateJob;

public record CreateJobResult(Guid Id, string Name, CreateJobResultLocation Location, string ServiceCategory);

public record CreateJobResultLocation(string County, string State);