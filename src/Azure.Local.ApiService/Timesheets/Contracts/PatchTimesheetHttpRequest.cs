namespace Azure.Local.ApiService.Timesheets.Contracts
{
    public class PatchTimesheetHttpRequest : AddTimesheetHttpRequest
    {
        // Concurrency control
        public string? ETag { get; set; }
    }
}
