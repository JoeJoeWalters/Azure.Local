using Azure.Local.ApiService.Timesheets.Contracts;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers.Validators
{
    public class PatchTimesheetHttpRequestValidator : AddTimesheetHttpRequestValidator
    {
        public PatchTimesheetHttpRequestValidator() : base()
        {
            RuleFor(x => ((PatchTimesheetHttpRequest)x).ETag)
                .MaximumLength(100).WithMessage("ETag must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(((PatchTimesheetHttpRequest)x).ETag));
        }
    }
}
