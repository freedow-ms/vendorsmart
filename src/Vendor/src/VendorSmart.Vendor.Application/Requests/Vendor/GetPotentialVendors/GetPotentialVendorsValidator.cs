using FluentValidation;
using VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;

namespace VendorSmart.Vendor.Application.Requests.Vendor.GetPotentialVendors;

public class GetPotentialVendorsRequestValidator : AbstractValidator<GetPotentialVendorsRequest>
{
    public GetPotentialVendorsRequestValidator()
    {
        RuleFor(request => request.ServiceCategory)
            .NotEmpty().WithMessage("Service Category is required.")
            .MaximumLength(100).WithMessage("Service Category must not exceed 100 characters.");

        RuleFor(request => request.County)
            .NotEmpty().WithMessage("County is required.")
            .MaximumLength(100).WithMessage("County must not exceed 100 characters.");

        RuleFor(request => request.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(100).WithMessage("State must not exceed 5 characters.");
    }
}
