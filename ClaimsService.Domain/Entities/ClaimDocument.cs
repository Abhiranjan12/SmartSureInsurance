using System.ComponentModel.DataAnnotations;

namespace ClaimsService.Domain.Entities
{
    public class ClaimDocument
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ClaimId { get; set; }

        public Claim Claim { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public long FileSizeBytes { get; set; }

        public DocumentStatus Status { get; set; } = DocumentStatus.Pending;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

    public enum DocumentStatus
    {
        Pending = 0,
        Verified = 1,
        Rejected = 2
    }
}
