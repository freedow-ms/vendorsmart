namespace VendorSmart.Vendor.Application.Requests.Vendor.CreateVendor;

public record CreateVendorResult(Guid Id, string Name, CreateVendorResultLocation Location, IEnumerable<CreateVendorResultServiceCategory> Services);

public record CreateVendorResultLocation(string County, string State);

public record CreateVendorResultServiceCategory(string Name, bool Compliant);