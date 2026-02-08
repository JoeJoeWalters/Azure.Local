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
                .NotEmpty().WithMessage("To is required.")
                .GreaterThan(x => x.From).WithMessage("To must be after From.");

            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("CreatedBy is required.")
                .MaximumLength(50).WithMessage("CreatedBy must not exceed 50 characters.");

            RuleFor(x => x.ManagerId)
                .MaximumLength(50).WithMessage("ManagerId must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.ManagerId));

            RuleFor(x => x.Comments)
                .MaximumLength(1000).WithMessage("Comments must not exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Comments));

            RuleForEach(x => x.Components).ChildRules(component =>
            {
                component.RuleFor(c => c.Description)
                    .MaximumLength(500).WithMessage("Component description must not exceed 500 characters.")
                    .When(c => !string.IsNullOrEmpty(c.Description));

                component.RuleFor(c => c.Units)
                    .GreaterThan(0).WithMessage("Component units must be greater than 0.")
                    .LessThanOrEqualTo(24).WithMessage("Component units cannot exceed 24 hours.");

                component.RuleFor(c => c.TimeCode)
                    .NotEmpty().WithMessage("Component TimeCode is required.")
                    .MaximumLength(50).WithMessage("Component TimeCode must not exceed 50 characters.");

                component.RuleFor(c => c.ProjectCode)
                    .NotEmpty().WithMessage("Component ProjectCode is required.")
                    .MaximumLength(50).WithMessage("Component ProjectCode must not exceed 50 characters.");
            });
        }
    }
}
