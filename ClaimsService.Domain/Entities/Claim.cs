using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsService.Domain.Entities
{
    public class Claim
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid PolicyId { get; set; }

        [Required]
        public string ClaimNumber { get; set; } = string.Empty;

        [Required]
        public string IncidentDescription { get; set; } = string.Empty;

        [Required]
        public DateTime IncidentDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ClaimAmount { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Draft;

        public string? RejectionReason { get; set; }

        public string? AdminRemarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ClaimDocument> Documents { get; set; } = new List<ClaimDocument>();
    }

    public enum ClaimStatus
    {
        Draft = 0,
        Submitted = 1,
        UnderReview = 2,
        Approved = 3,
        Rejected = 4,
        Closed = 5
    }
}
