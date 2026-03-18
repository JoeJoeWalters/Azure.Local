using System.ComponentModel.DataAnnotations;

namespace Azure.Local.ApiService.Timesheets.Contracts.V1
{
    public class AddTimesheetHttpRequestV1
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public required string PersonId { get; set; }

        [Required]
        public required DateTime From { get; set; }

        [Required]
        public required DateTime To { get; set; }

        public List<TimesheetHttpRequestComponentV1> Components { get; set; } = [];

        // Metadata
        [MaxLength(50)]
        public string? ManagerId { get; set; }

        [MaxLength(1000)]
        public string? Comments { get; set; }

        // Audit - CreatedBy will be set server-side from authenticated user
        [Required]
        [MaxLength(50)]
        public required string CreatedBy { get; set; }
    }
}
