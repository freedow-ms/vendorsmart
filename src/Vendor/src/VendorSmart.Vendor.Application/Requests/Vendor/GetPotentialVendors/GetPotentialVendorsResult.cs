namespace VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;

public record GetPotentialVendorsResult(List<GetPotentialVendorsResultVendor> Vendors);

public record GetPotentialVendorsResultVendor(Guid Id, string Name, bool IsCompliant);