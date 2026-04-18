namespace PolicyService.Application.DTOs
{
    public class PolicyTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePremium { get; set; }
        public int CoverageYears { get; set; }
        public bool IsActive { get; set; }
    }
}
