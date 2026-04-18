using System.ComponentModel.DataAnnotations;

namespace AdminService.Application.DTOs
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ReportType { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }

    public class GenerateReportDto
    {
        [Required]
        public string ReportType { get; set; } = string.Empty;
    }
}
