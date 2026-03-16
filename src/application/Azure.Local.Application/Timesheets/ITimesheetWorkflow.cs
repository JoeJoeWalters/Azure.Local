using Azure.Local.Application.Timesheets.Workflows;
using Azure.Local.Domain.Timesheets;

namespace Azure.Local.Application.Timesheets
{
    public interface ITimesheetWorkflow
    {
        TimesheetWorkflowResult Submit(TimesheetItem timesheet, string submittedBy);
        TimesheetWorkflowResult Approve(TimesheetItem timesheet, string approvedBy);
        TimesheetWorkflowResult Reject(TimesheetItem timesheet, string rejectedBy, string reason);
        TimesheetWorkflowResult Recall(TimesheetItem timesheet, string recalledBy);
        TimesheetWorkflowResult Reopen(TimesheetItem timesheet, string reopenedBy);
    }
}
