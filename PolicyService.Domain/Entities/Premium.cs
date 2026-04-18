using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyService.Domain.Entities
{
    public class Premium
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PolicyTypeId { get; set; }

        public PolicyType PolicyType { get; set; } = null!;

        public int CustomerAge { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CoverageAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CalculatedPremium { get; set; }
        public int DurationYears { get; set; }
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
}
