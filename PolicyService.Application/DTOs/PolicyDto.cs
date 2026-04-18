namespace PolicyService.Application.DTOs
{
    public class PolicyDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string PolicyTypeName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
