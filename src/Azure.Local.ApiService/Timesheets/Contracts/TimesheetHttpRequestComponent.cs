namespace Azure.Local.ApiService.Timesheets.Contracts
{
    public class TimesheetHttpRequestComponent
    {
        public required double Units { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public required string Code { get; set; }
    }
}
