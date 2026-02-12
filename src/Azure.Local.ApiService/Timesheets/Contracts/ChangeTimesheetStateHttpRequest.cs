using Azure.Local.Domain.Timesheets;

namespace Azure.Local.ApiService.Timesheets.Contracts
{
    /// <summary>
    /// Request to change the state of a timesheet
    /// </summary>
    public class ChangeTimesheetStateHttpRequest
    {
        /// <summary>
        /// The target state action to perform
        /// </summary>
        public TimesheetStateAction State { get; set; }

        /// <summary>
        /// Optional comments for the state change (required for Reject)
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Optional ETag for concurrency control
        /// </summary>
        public string? ETag { get; set; }
    }
}
