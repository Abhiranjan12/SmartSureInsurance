using System.ComponentModel.DataAnnotations;

namespace PolicyService.Application.DTOs
{
    public class BuyPolicyDto
    {
        [Required]
        public Guid PolicyTypeId { get; set; }

        [Required]
        public decimal CoverageAmount { get; set; }

        [Required]
        public int DurationYears { get; set; }

        [Required]
        [Range(18, 100)]
        public int CustomerAge { get; set; }
    }
}
