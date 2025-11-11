using Azure.Local.ApiService.Test.Contracts;
using FluentValidation;

namespace Azure.Local.ApiService.Test.Controllers.Validators
{
    public class PatchTimesheetHttpRequestValidator : AddTimesheetHttpRequestValidator
    {
        public PatchTimesheetHttpRequestValidator() : base()
        {
        }
    }
}
