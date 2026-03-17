using Azure.Local.Application.Timesheets.Validators;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Tests.Unit.Timesheets
{
    [ExcludeFromCodeCoverage]
    public class TimesheetBusinessRulesUnitTests
    {
        private readonly TimesheetBusinessRules _sut = new();

        [Fact]
        public void Validate_ShouldSucceed_ForValidTimesheet()
        {
            var timesheet = CreateTimesheet(
                from: new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                to: new DateTime(2026, 1, 11, 0, 0, 0, DateTimeKind.Utc),
                components:
                [
                    CreateComponent(8, new DateTime(2026, 1, 10, 9, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 17, 0, 0, DateTimeKind.Utc))
                ]);

            var result = _sut.Validate(timesheet);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_ShouldFail_WhenDateOrderInvalid_AndNoComponents()
        {
            var timesheet = CreateTimesheet(
                from: new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                to: new DateTime(2026, 1, 9, 0, 0, 0, DateTimeKind.Utc),
                components: []);

            var result = _sut.Validate(timesheet);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("Timesheet 'To' date must be after 'From' date.");
            result.Errors.Should().Contain("Timesheet must have at least one component entry.");
        }

        [Fact]
        public void Validate_ShouldFail_WhenTimesheetPeriodExceeds31Days()
        {
            var from = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc);
            var timesheet = CreateTimesheet(
                from,
                to,
                [CreateComponent(8, from.AddDays(1).AddHours(9), from.AddDays(1).AddHours(17))]);

            var result = _sut.Validate(timesheet);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("Timesheet period cannot exceed 31 days.");
        }

        [Fact]
        public void Validate_ShouldFail_ForComponentRules_Overlaps_AndUnitsPerDay()
        {
            var from = new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2026, 1, 12, 0, 0, 0, DateTimeKind.Utc);
            var components = new List<TimesheetComponentItem>
            {
                // outside range (from), missing codes, locked, non-positive units
                new()
                {
                    Units = 0,
                    From = from.AddHours(-1),
                    To = from,
                    TimeCode = "",
                    ProjectCode = "",
                    IsLocked = true
                },
                // outside range (to) and units > 24
                new()
                {
                    Units = 25,
                    From = from.AddHours(10),
                    To = to.AddHours(1),
                    TimeCode = "DEV",
                    ProjectCode = "PRJ"
                },
                // invalid time order
                new()
                {
                    Units = 1,
                    From = from.AddHours(14),
                    To = from.AddHours(13),
                    TimeCode = "DEV",
                    ProjectCode = "PRJ"
                },
                // overlaps with 2nd component
                new()
                {
                    Units = 2,
                    From = from.AddHours(11),
                    To = from.AddHours(12),
                    TimeCode = "QA",
                    ProjectCode = "PRJ"
                },
                // non-overlapping component to exercise false overlap branch
                new()
                {
                    Units = 2,
                    From = from.AddDays(1).AddHours(8),
                    To = from.AddDays(1).AddHours(10),
                    TimeCode = "DOC",
                    ProjectCode = "PRJ"
                }
            };

            var timesheet = CreateTimesheet(from, to, components);

            var result = _sut.Validate(timesheet);

            result.IsValid.Should().BeFalse();
            var errors = string.Join(" | ", result.Errors);
            errors.Should().Contain("Units must be greater than 0.");
            errors.Should().Contain("Units cannot exceed 24 hours.");
            errors.Should().Contain("Component time period must be within timesheet period.");
            errors.Should().Contain("'To' time must be after 'From' time.");
            errors.Should().Contain("TimeCode is required.");
            errors.Should().Contain("ProjectCode is required.");
            errors.Should().Contain("Component is locked and cannot be modified.");
            errors.Should().Contain("Found");
            errors.Should().Contain("exceeds 24 hours");
        }

        private static TimesheetItem CreateTimesheet(DateTime from, DateTime to, List<TimesheetComponentItem> components)
            => new()
            {
                PersonId = "person-1",
                CreatedBy = "person-1",
                From = from,
                To = to,
                Status = TimesheetStatus.Draft,
                Components = components
            };

        private static TimesheetComponentItem CreateComponent(double units, DateTime from, DateTime to)
            => new()
            {
                Units = units,
                From = from,
                To = to,
                TimeCode = "DEV",
                ProjectCode = "PRJ",
                IsBillable = true
            };
    }
}
