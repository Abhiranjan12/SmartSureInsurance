using MassTransit;
using Microsoft.Extensions.Logging;
using SmartSure.Contracts.Events;

namespace ClaimsService.API.Consumers
{
    public class PolicyPurchaseFailedConsumer : IConsumer<PolicyPurchaseFailedEvent>
    {
        private readonly ILogger<PolicyPurchaseFailedConsumer> _logger;

        public PolicyPurchaseFailedConsumer(ILogger<PolicyPurchaseFailedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PolicyPurchaseFailedEvent> context)
        {
            var evt = context.Message;
            _logger.LogWarning(
                "Policy purchase failed for Customer {CustomerId}. Reason: {Reason}",
                evt.CustomerId, evt.Reason);

            return Task.CompletedTask;
        }
    }
}
