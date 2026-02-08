namespace Azure.Local.Domain.Timesheets
{
    /// <summary>
    /// Timesheet workflow status
    /// </summary>
    public enum TimesheetStatus
    {
        /// <summary>
        /// Initial state - being edited
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Submitted for approval
        /// </summary>
        Submitted = 1,

        /// <summary>
        /// Approved by manager
        /// </summary>
        Approved = 2,

        /// <summary>
        /// Rejected by manager
        /// </summary>
        Rejected = 3,

        /// <summary>
        /// Recalled by submitter
        /// </summary>
        Recalled = 4
    }
}
