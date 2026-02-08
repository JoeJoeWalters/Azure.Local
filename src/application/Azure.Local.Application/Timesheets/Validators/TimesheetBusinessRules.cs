using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets.Validators
{
    /// <summary>
    /// Business rules validation for timesheets
    /// </summary>
    public class TimesheetBusinessRules
    {
        /// <summary>
        /// Validate a timesheet against all business rules
        /// </summary>
        public ValidationResult Validate(TimesheetItem timesheet)
        {
            var errors = new List<string>();

            // Date range validation
            if (timesheet.From >= timesheet.To)
            {
                errors.Add("Timesheet 'To' date must be after 'From' date.");
            }

            // Date range not too long (e.g., max 31 days)
            if ((timesheet.To - timesheet.From).TotalDays > 31)
            {
                errors.Add("Timesheet period cannot exceed 31 days.");
            }

            // Must have components to submit
            if (timesheet.Components.Count == 0)
            {
                errors.Add("Timesheet must have at least one component entry.");
            }

            // Validate each component
            for (int i = 0; i < timesheet.Components.Count; i++)
            {
                var component = timesheet.Components[i];
                var componentErrors = ValidateComponent(component, timesheet);
                
                foreach (var error in componentErrors)
                {
                    errors.Add($"Component {i + 1}: {error}");
                }
            }

            // Check for overlapping components
            var overlaps = FindOverlappingComponents(timesheet.Components);
            if (overlaps.Any())
            {
                errors.Add($"Found {overlaps.Count} overlapping time entries.");
            }

            // Total units validation (max 24 hours per day)
            var totalUnitsPerDay = CalculateTotalUnitsPerDay(timesheet);
            foreach (var (date, units) in totalUnitsPerDay)
            {
                if (units > 24)
                {
                    errors.Add($"Total units for {date:yyyy-MM-dd} exceeds 24 hours ({units:F2} hours).");
                }
            }

            return errors.Count == 0
                ? ValidationResult.Success()
                : ValidationResult.Failure(errors);
        }

        /// <summary>
        /// Validate a single component
        /// </summary>
        private List<string> ValidateComponent(TimesheetComponentItem component, TimesheetItem timesheet)
        {
            var errors = new List<string>();

            // Time validation
            if (component.From >= component.To)
            {
                errors.Add("'To' time must be after 'From' time.");
            }

            // Units validation
            if (component.Units <= 0)
            {
                errors.Add("Units must be greater than 0.");
            }

            if (component.Units > 24)
            {
                errors.Add("Units cannot exceed 24 hours.");
            }

            // Component must be within timesheet period
            if (component.From < timesheet.From || component.To > timesheet.To)
            {
                errors.Add("Component time period must be within timesheet period.");
            }

            // Required codes
            if (string.IsNullOrWhiteSpace(component.TimeCode))
            {
                errors.Add("TimeCode is required.");
            }

            if (string.IsNullOrWhiteSpace(component.ProjectCode))
            {
                errors.Add("ProjectCode is required.");
            }

            // Locked components cannot be edited
            if (component.IsLocked)
            {
                errors.Add("Component is locked and cannot be modified.");
            }

            return errors;
        }

        /// <summary>
        /// Find overlapping components
        /// </summary>
        private List<(TimesheetComponentItem, TimesheetComponentItem)> FindOverlappingComponents(
            List<TimesheetComponentItem> components)
        {
            var overlaps = new List<(TimesheetComponentItem, TimesheetComponentItem)>();

            for (int i = 0; i < components.Count; i++)
            {
                for (int j = i + 1; j < components.Count; j++)
                {
                    var c1 = components[i];
                    var c2 = components[j];

                    // Check if time periods overlap
                    if (c1.From < c2.To && c2.From < c1.To)
                    {
                        overlaps.Add((c1, c2));
                    }
                }
            }

            return overlaps;
        }

        /// <summary>
        /// Calculate total units per day
        /// </summary>
        private Dictionary<DateTime, double> CalculateTotalUnitsPerDay(TimesheetItem timesheet)
        {
            var unitsPerDay = new Dictionary<DateTime, double>();

            foreach (var component in timesheet.Components)
            {
                var date = component.From.Date;
                
                if (!unitsPerDay.ContainsKey(date))
                {
                    unitsPerDay[date] = 0;
                }

                unitsPerDay[date] += component.Units;
            }

            return unitsPerDay;
        }
    }
}
