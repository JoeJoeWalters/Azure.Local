namespace Azure.Local.ApiService.Timesheets.Rendering
{
    public sealed class TimesheetRenderResult
    {
        public required string ContentType { get; init; }
        public required byte[] Content { get; init; }
        public string? FileDownloadName { get; init; }
    }
}
