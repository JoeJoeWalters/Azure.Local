using Azure.Local.ApiService.Test.Contracts;
using FluentValidation;

namespace Azure.Local.ApiService.Test.Controllers.Validators
{
    public class AddTimesheetHttpRequestValidator : AbstractValidator<AddTimesheetHttpRequest>
    {
        public AddTimesheetHttpRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .MaximumLength(50).WithMessage("Id must not exceed 50 characters.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
