using System.ComponentModel.DataAnnotations;

namespace ClaimsService.Application.DTOs
{
    public class CreateClaimDto
    {
        [Required]
        public Guid PolicyId { get; set; }

        [Required]
        public string IncidentDescription { get; set; } = string.Empty;

        [Required]
        public DateTime IncidentDate { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Claim amount must be greater than 0.")]
        public decimal ClaimAmount { get; set; }
    }
}
