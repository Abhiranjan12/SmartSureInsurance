namespace PolicyService.Application.DTOs
{
    public class PremiumResultDto
    {
        public Guid PolicyTypeId { get; set; }
        public string PolicyTypeName { get; set; } = string.Empty;
        public decimal CoverageAmount { get; set; }
        public int DurationYears { get; set; }
        public decimal CalculatedPremium { get; set; }
        public decimal MonthlyPremium { get; set; }
    }
}
