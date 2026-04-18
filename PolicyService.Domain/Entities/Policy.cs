using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyService.Domain.Entities
{
    public class Policy
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid PolicyTypeId { get; set; }

        public PolicyType PolicyType { get; set; } = null!;

        [Required]
        public string PolicyNumber { get; set; } = string.Empty;

        public PolicyStatus Status { get; set; } = PolicyStatus.Draft;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CoverageAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public enum PolicyStatus
    {
        Draft = 0,
        Active = 1,
        Expired = 2,
        Cancelled = 3
    }
}
