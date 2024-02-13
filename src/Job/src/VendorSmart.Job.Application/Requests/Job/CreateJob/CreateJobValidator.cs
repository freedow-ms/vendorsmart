using FluentValidation;

namespace VendorSmart.Job.Application.Requests.Job.CreateJob;

public class CreateJobRequestValidator : AbstractValidator<CreateJobRequest>
{
    public CreateJobRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(4, 100).WithMessage("Job name must be between 5 and 100 characters.");

        RuleFor(request => request.ServiceCategory)
            .NotEmpty().WithMessage("Service Category is required.")
            .MaximumLength(100).WithMessage("Service Category must not exceed 100 characters.");

        // Validate nested Location object
        RuleFor(request => request.Location)
            .NotNull().WithMessage("Location is required.")
            .SetValidator(new CreateJobRequestLocationValidator());
    }
}
public class CreateJobRequestLocationValidator : AbstractValidator<CreateJobRequestLocation>
{
    public CreateJobRequestLocationValidator()
    {
        RuleFor(location => location.County)
            .NotEmpty().WithMessage("County is required.")
            .Length(2, 100).WithMessage("County must be between 2 and 100 characters.");

        RuleFor(location => location.State)
            .NotEmpty().WithMessage("State is required.")
            .Length(2, 5).WithMessage("State must be between 2 and 5 characters.");
    }
}