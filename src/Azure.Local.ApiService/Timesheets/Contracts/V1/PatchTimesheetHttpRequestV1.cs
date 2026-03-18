namespace Azure.Local.ApiService.Timesheets.Contracts.V1
{
    public class PatchTimesheetHttpRequestV1 : AddTimesheetHttpRequestV1
    {
        // Concurrency control
        public string? ETag { get; set; }
    }
}
