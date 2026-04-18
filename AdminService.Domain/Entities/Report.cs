using System.ComponentModel.DataAnnotations;

namespace AdminService.Domain.Entities
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ReportType { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public Guid GeneratedBy { get; set; }
    }
}
