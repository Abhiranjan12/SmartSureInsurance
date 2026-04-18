namespace SmartSure.Contracts.Events
{
    public class PolicyPurchaseFailedEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid CustomerId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
