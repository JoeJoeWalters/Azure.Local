namespace Azure.Local.Domain.Timesheets
{
    /// <summary>
    /// Represents a state change action for a timesheet
    /// </summary>
    public enum TimesheetStateAction
    {
        /// <summary>
        /// Submit timesheet for approval
        /// </summary>
        Submit = 0,

        /// <summary>
        /// Approve timesheet
        /// </summary>
        Approve = 1,

        /// <summary>
        /// Reject timesheet
        /// </summary>
        Reject = 2,

        /// <summary>
        /// Recall submitted timesheet
        /// </summary>
        Recall = 3
    }
}
