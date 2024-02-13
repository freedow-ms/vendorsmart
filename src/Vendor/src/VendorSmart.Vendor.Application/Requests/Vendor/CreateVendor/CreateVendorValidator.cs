using FluentValidation;

namespace VendorSmart.Vendor.Application.Requests.Vendor.CreateVendor;
public class CreateVendorRequestValidator : AbstractValidator<CreateVendorRequest>
{
    public CreateVendorRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Vendor name is required.")
            .Length(2, 100).WithMessage("Vendor name must be between 2 and 100 characters.");

        RuleFor(request => request.ServiceCategories)
            .NotEmpty().WithMessage("At least one service category is required.")
            .Must(sc => sc.All(s => !string.IsNullOrEmpty(s.Name))).WithMessage("Service category names cannot be empty.");

        RuleForEach(request => request.ServiceCategories).SetValidator(new CreateVendorRequestServiceCategoryValidator());

        RuleFor(request => request.Location)
            .NotNull().WithMessage("Location is required.")
            .SetValidator(new CreateVendorRequestLocationValidator());
    }
}

public class CreateVendorRequestServiceCategoryValidator : AbstractValidator<CreateVendorRequestServiceCategory>
{
    public CreateVendorRequestServiceCategoryValidator()
    {
        RuleFor(sc => sc.Name)
            .NotEmpty().WithMessage("Service category name is required.");
    }
}

public class CreateVendorRequestLocationValidator : AbstractValidator<CreateVendorRequestLocation>
{
    public CreateVendorRequestLocationValidator()
    {
        RuleFor(location => location.County)
            .NotEmpty().WithMessage("County is required.")
            .Length(2, 100).WithMessage("County must be between 2 and 100 characters.");

        RuleFor(location => location.State)
            .NotEmpty().WithMessage("State is required.")
            .Length(2, 5).WithMessage("State must be between 2 and 5 characters.");
    }
}
