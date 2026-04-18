using System.ComponentModel.DataAnnotations;

namespace ClaimsService.Application.DTOs
{
    public class UpdateClaimStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public string? AdminRemarks { get; set; }
    }
}
