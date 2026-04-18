using System.ComponentModel.DataAnnotations;

namespace ClaimsService.Application.DTOs
{
    public class UpdateDocumentStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
