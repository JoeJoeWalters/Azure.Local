namespace Azure.Local.Application.Timesheets.Workflows
{
    /// <summary>
    /// Result of a workflow operation
    /// </summary>
    public class TimesheetWorkflowResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];

        public static TimesheetWorkflowResult Success(string message)
            => new() { IsSuccess = true, Message = message };

        public static TimesheetWorkflowResult Failure(string error)
            => new() { IsSuccess = false, Errors = [error] };

        public static TimesheetWorkflowResult Failure(List<string> errors)
            => new() { IsSuccess = false, Errors = errors };
    }
}
