namespace Azure.Local.ApiService.Test.Contracts
{
    public class AddTimesheetHttpRequest
    {
        public string Id { get; set; } = string.Empty;
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;
        public List<AddTimesheetHttpRequestComponent> Components { get; set; } = new List<AddTimesheetHttpRequestComponent>();
    }

    public class AddTimesheetHttpRequestComponent
    {
        public double Units { get; set; } = 0.0D;
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;
    }
}
