namespace Azure.Local.ApiService.Timesheets.Contracts
{
    /// <summary>
    /// Request to approve a timesheet
    /// </summary>
    public class ApproveTimesheetHttpRequest
    {
        public string? Comments { get; set; }
        public string? ETag { get; set; }
    }
}
