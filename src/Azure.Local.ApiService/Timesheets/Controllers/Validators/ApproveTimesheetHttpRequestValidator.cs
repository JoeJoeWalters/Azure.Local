using Azure.Local.ApiService.Timesheets.Contracts;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers.Validators
{
    public class ApproveTimesheetHttpRequestValidator : AbstractValidator<ApproveTimesheetHttpRequest>
    {
        public ApproveTimesheetHttpRequestValidator()
        {
            RuleFor(x => x.Comments)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Comments))
                .WithMessage("Comments must not exceed 1000 characters.");

            RuleFor(x => x.ETag)
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.ETag))
                .WithMessage("ETag must not be empty if provided.");
        }
    }
}
