namespace Azure.Local.Application.Timesheets.Validators
{
    /// <summary>
    /// Validation result
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = [];

        public static ValidationResult Success()
            => new() { IsValid = true };

        public static ValidationResult Failure(string error)
            => new() { IsValid = false, Errors = [error] };

        public static ValidationResult Failure(List<string> errors)
            => new() { IsValid = false, Errors = errors };
    }
}
