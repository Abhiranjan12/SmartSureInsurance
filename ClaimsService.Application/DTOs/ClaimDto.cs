namespace ClaimsService.Application.DTOs
{
    public class ClaimDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PolicyId { get; set; }
        public string ClaimNumber { get; set; } = string.Empty;
        public string IncidentDescription { get; set; } = string.Empty;
        public DateTime IncidentDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public string? AdminRemarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ClaimDocumentDto> Documents { get; set; } = new List<ClaimDocumentDto>();
    }
}
