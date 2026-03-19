using Azure.Local.ApiService.Timesheets.Contracts;
using Azure.Local.Domain.Timesheets;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers.Validators
{
    public class ChangeTimesheetStateHttpRequestValidator : AbstractValidator<ChangeTimesheetStateHttpRequest>
    {
        public ChangeTimesheetStateHttpRequestValidator()
        {
            RuleFor(x => x.State)
                .IsInEnum()
                .WithMessage("Invalid state action.");

            RuleFor(x => x.Comments)
                .NotEmpty()
                .WithMessage("Comments are required when rejecting a timesheet.")
                .When(x => x.State == TimesheetStateAction.Reject);

            RuleFor(x => x.Comments)
                .MaximumLength(1000)
                .WithMessage("Comments must not exceed 1000 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Comments));

            RuleFor(x => x.ETag)
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.ETag))
                .WithMessage("ETag must not be empty if provided.");
        }
    }
}
