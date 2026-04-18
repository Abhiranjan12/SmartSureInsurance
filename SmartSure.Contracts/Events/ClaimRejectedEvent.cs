namespace SmartSure.Contracts.Events
{
    public class ClaimRejectedEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid ClaimId { get; set; }
        public Guid CustomerId { get; set; }
        public string ClaimNumber { get; set; } = string.Empty;
        public string RejectionReason { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
