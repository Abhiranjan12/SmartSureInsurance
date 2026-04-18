using System.ComponentModel.DataAnnotations;

namespace PolicyService.Application.DTOs
{
    public class UpdatePolicyStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
