namespace SmartSure.Contracts.Events
{
    public class PolicyPurchasedEvent
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public Guid PolicyId { get; set; }
        public Guid CustomerId { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string PolicyTypeName { get; set; } = string.Empty;
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
