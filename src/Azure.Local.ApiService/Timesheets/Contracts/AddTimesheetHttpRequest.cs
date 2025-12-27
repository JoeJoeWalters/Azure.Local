namespace Azure.Local.ApiService.Timesheets.Contracts
{
    public class AddTimesheetHttpRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string PersonId { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public List<TimesheetHttpRequestComponent> Components { get; set; } = [];
    }
}
