using Azure.Local.Application.Timesheets.Validators;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets.Workflows
{
    /// <summary>
    /// Manages timesheet status transitions and business rules
    /// </summary>
    public class TimesheetWorkflow
    {
        private readonly TimesheetBusinessRules _businessRules = new();

        /// <summary>
        /// Submit a timesheet for approval
        /// </summary>
        public TimesheetWorkflowResult Submit(TimesheetItem timesheet, string submittedBy)
        {
            // Validate can submit
            if (!timesheet.CanSubmit())
            {
                return TimesheetWorkflowResult.Failure(
                    $"Timesheet cannot be submitted. Current status: {timesheet.Status}");
            }

            // Validate business rules
            var validationResult = _businessRules.Validate(timesheet);
            if (!validationResult.IsValid)
            {
                return TimesheetWorkflowResult.Failure(validationResult.Errors);
            }

            // Perform transition
            timesheet.Status = TimesheetStatus.Submitted;
            timesheet.SubmittedDate = DateTime.UtcNow;
            timesheet.ModifiedDate = DateTime.UtcNow;
            timesheet.ModifiedBy = submittedBy;

            return TimesheetWorkflowResult.Success("Timesheet submitted successfully.");
        }

        /// <summary>
        /// Approve a timesheet
        /// </summary>
        public TimesheetWorkflowResult Approve(TimesheetItem timesheet, string approvedBy)
        {
            // Validate can approve
            if (!timesheet.CanApprove())
            {
                return TimesheetWorkflowResult.Failure(
                    $"Timesheet cannot be approved. Current status: {timesheet.Status}");
            }

            // Business rule: Approver cannot be the submitter
            if (timesheet.PersonId == approvedBy)
            {
                return TimesheetWorkflowResult.Failure(
                    "You cannot approve your own timesheet.");
            }

            // Perform transition
            timesheet.Status = TimesheetStatus.Approved;
            timesheet.ApprovedDate = DateTime.UtcNow;
            timesheet.ApprovedBy = approvedBy;
            timesheet.ModifiedDate = DateTime.UtcNow;
            timesheet.ModifiedBy = approvedBy;

            // Lock all components
            foreach (var component in timesheet.Components)
            {
                component.IsLocked = true;
            }

            return TimesheetWorkflowResult.Success("Timesheet approved successfully.");
        }

        /// <summary>
        /// Reject a timesheet
        /// </summary>
        public TimesheetWorkflowResult Reject(TimesheetItem timesheet, string rejectedBy, string reason)
        {
            // Validate can reject
            if (!timesheet.CanReject())
            {
                return TimesheetWorkflowResult.Failure(
                    $"Timesheet cannot be rejected. Current status: {timesheet.Status}");
            }

            // Validate reason provided
            if (string.IsNullOrWhiteSpace(reason))
            {
                return TimesheetWorkflowResult.Failure(
                    "Rejection reason is required.");
            }

            // Perform transition
            timesheet.Status = TimesheetStatus.Rejected;
            timesheet.RejectedDate = DateTime.UtcNow;
            timesheet.RejectedBy = rejectedBy;
            timesheet.RejectionReason = reason;
            timesheet.ModifiedDate = DateTime.UtcNow;
            timesheet.ModifiedBy = rejectedBy;

            return TimesheetWorkflowResult.Success("Timesheet rejected.");
        }

        /// <summary>
        /// Recall a submitted timesheet
        /// </summary>
        public TimesheetWorkflowResult Recall(TimesheetItem timesheet, string recalledBy)
        {
            // Validate can recall
            if (!timesheet.CanRecall())
            {
                return TimesheetWorkflowResult.Failure(
                    $"Timesheet cannot be recalled. Current status: {timesheet.Status}");
            }

            // Business rule: Only submitter can recall
            if (timesheet.PersonId != recalledBy && timesheet.CreatedBy != recalledBy)
            {
                return TimesheetWorkflowResult.Failure(
                    "Only the timesheet owner can recall it.");
            }

            // Perform transition
            timesheet.Status = TimesheetStatus.Recalled;
            timesheet.ModifiedDate = DateTime.UtcNow;
            timesheet.ModifiedBy = recalledBy;

            return TimesheetWorkflowResult.Success("Timesheet recalled. Status changed to Recalled.");
        }

        /// <summary>
        /// Reopen a recalled or rejected timesheet to draft
        /// </summary>
        public TimesheetWorkflowResult Reopen(TimesheetItem timesheet, string reopenedBy)
        {
            // Only recalled or rejected can be reopened
            if (timesheet.Status != TimesheetStatus.Recalled && 
                timesheet.Status != TimesheetStatus.Rejected)
            {
                return TimesheetWorkflowResult.Failure(
                    $"Only recalled or rejected timesheets can be reopened. Current status: {timesheet.Status}");
            }

            // Perform transition
            var previousStatus = timesheet.Status;
            timesheet.Status = TimesheetStatus.Draft;
            timesheet.ModifiedDate = DateTime.UtcNow;
            timesheet.ModifiedBy = reopenedBy;

            // Unlock components if they were locked
            foreach (var component in timesheet.Components)
            {
                component.IsLocked = false;
            }

            return TimesheetWorkflowResult.Success(
                $"Timesheet reopened from {previousStatus} to Draft.");
        }
    }
}
