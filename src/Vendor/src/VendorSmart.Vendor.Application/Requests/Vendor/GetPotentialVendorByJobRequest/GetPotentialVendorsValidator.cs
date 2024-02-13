using FluentValidation;
using VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;

namespace VendorSmart.Vendor.Application.Requests.Vendor.GetPotentialVendors;

public class GetPotentialVendorsByJobRequestValidator : AbstractValidator<GetPotentialVendorsByJobRequest>
{
    public GetPotentialVendorsByJobRequestValidator()
    {
        RuleFor(request => request.JobId)
            .NotEmpty().WithMessage("JobId is required.");
    }
}
