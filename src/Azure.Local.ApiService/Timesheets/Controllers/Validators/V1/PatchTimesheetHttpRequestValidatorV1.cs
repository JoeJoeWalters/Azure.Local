using Azure.Local.ApiService.Timesheets.Contracts.V1;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers.Validators.V1
{
    public class PatchTimesheetHttpRequestValidatorV1 : AddTimesheetHttpRequestValidatorV1
    {
        public PatchTimesheetHttpRequestValidatorV1() : base()
        {
            RuleFor(x => ((PatchTimesheetHttpRequestV1)x).ETag)
                .MaximumLength(100).WithMessage("ETag must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(((PatchTimesheetHttpRequestV1)x).ETag));
        }
    }
}
