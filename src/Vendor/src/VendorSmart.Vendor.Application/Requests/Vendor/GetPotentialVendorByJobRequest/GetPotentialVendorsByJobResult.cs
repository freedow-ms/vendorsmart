namespace VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;

public record GetPotentialVendorsByJobResult(List<GetPotentialVendorsByJobResultVendor> Vendors);

public record GetPotentialVendorsByJobResultVendor(Guid Id, string Name, bool IsCompliant);