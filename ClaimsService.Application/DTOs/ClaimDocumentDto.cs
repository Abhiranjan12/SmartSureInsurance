namespace ClaimsService.Application.DTOs
{
    public class ClaimDocumentDto
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
