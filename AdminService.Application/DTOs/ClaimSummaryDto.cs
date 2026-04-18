using System.ComponentModel.DataAnnotations;

namespace AdminService.Application.DTOs
{
    public class ClaimSummaryDto
    {
        public Guid Id { get; set; }
        public string ClaimNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public Guid PolicyId { get; set; }
        public string IncidentDescription { get; set; } = string.Empty;
        public DateTime IncidentDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public string? AdminRemarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateClaimStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public string? AdminRemarks { get; set; }
    }
}
