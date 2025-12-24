using Azure.Local.ApiService.Timesheets.Contracts;
using FluentValidation;

namespace Azure.Local.ApiService.Timesheets.Controllers.Validators
{
    public class AddTimesheetHttpRequestValidator : AbstractValidator<AddTimesheetHttpRequest>
    {
        public AddTimesheetHttpRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .MaximumLength(50).WithMessage("Id must not exceed 50 characters.");
            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage("Person Id is required.")
                .MaximumLength(50).WithMessage("Person Id must not exceed 50 characters.");
            RuleFor(x => x.From)
                .NotEmpty().WithMessage("From is required.");
            RuleFor(x => x.To)
                .NotEmpty().WithMessage("To is required.");
            //  .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
