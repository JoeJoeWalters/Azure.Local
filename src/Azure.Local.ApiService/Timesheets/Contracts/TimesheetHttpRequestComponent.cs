using System.ComponentModel.DataAnnotations;

namespace Azure.Local.ApiService.Timesheets.Contracts
{
    public class TimesheetHttpRequestComponent
    {
        // Identity (optional for new, required for updates)
        public string? Id { get; set; }

        // Time & Duration
        [Required]
        [Range(0.01, 24.0)]
        public required double Units { get; set; }

        [Required]
        public required DateTime From { get; set; }

        [Required]
        public required DateTime To { get; set; }

        // Coding
        [Required]
        [MaxLength(50)]
        public required string TimeCode { get; set; }

        [Required]
        [MaxLength(50)]
        public required string ProjectCode { get; set; }

        // Work Details
        [MaxLength(500)]
        public string? Description { get; set; }

        public string? WorkType { get; set; }  // Maps to WorkType enum

        // Classification
        public bool? IsBillable { get; set; } = true;
    }
}
