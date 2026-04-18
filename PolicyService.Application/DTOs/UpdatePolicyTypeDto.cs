using System.ComponentModel.DataAnnotations;

namespace PolicyService.Application.DTOs
{
    public class UpdatePolicyTypeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal BasePremium { get; set; }

        [Required]
        public int CoverageYears { get; set; }

        public bool IsActive { get; set; }
    }
}
