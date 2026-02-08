namespace Azure.Local.ApiService.Timesheets.Contracts
{
    /// <summary>
    /// Request to recall a submitted timesheet
    /// </summary>
    public class RecallTimesheetHttpRequest
    {
        public string? Reason { get; set; }
        public string? ETag { get; set; }
    }
}
