using Azure.Local.ApiService.Timesheets.Contracts;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers.Validators
{
    public class RejectTimesheetHttpRequestValidator : AbstractValidator<RejectTimesheetHttpRequest>
    {
        public RejectTimesheetHttpRequestValidator()
        {
            RuleFor(x => x.RejectionReason)
                .NotEmpty()
                .WithMessage("Rejection reason is required.")
                .MaximumLength(1000)
                .WithMessage("Rejection reason must not exceed 1000 characters.");

            RuleFor(x => x.ETag)
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.ETag))
                .WithMessage("ETag must not be empty if provided.");
        }
    }
}
