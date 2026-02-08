namespace Azure.Local.ApiService.Timesheets.Contracts
{
    /// <summary>
    /// Request to reject a timesheet
    /// </summary>
    public class RejectTimesheetHttpRequest
    {
        public string RejectionReason { get; set; } = string.Empty;
        public string? ETag { get; set; }
    }
}
