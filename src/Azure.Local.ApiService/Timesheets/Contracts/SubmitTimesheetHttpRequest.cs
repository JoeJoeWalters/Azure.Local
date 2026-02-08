namespace Azure.Local.ApiService.Timesheets.Contracts
{
    /// <summary>
    /// Request to submit a timesheet for approval
    /// </summary>
    public class SubmitTimesheetHttpRequest
    {
        public string? Comments { get; set; }
        public string? ETag { get; set; }
    }
}
