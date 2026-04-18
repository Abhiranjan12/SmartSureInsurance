namespace SmartSure.Contracts.Events
{
    public class ClaimSubmittedEvent
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public Guid ClaimId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PolicyId { get; set; }
        public string ClaimNumber { get; set; } = string.Empty;
        public decimal ClaimAmount { get; set; }
        public string IncidentDescription { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
