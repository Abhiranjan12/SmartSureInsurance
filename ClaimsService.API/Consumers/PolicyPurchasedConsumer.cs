using MassTransit;
using Microsoft.Extensions.Logging;
using SmartSure.Contracts.Events;

namespace ClaimsService.API.Consumers
{
    public class PolicyPurchasedConsumer : IConsumer<PolicyPurchasedEvent>
    {
        private readonly ILogger<PolicyPurchasedConsumer> _logger;

        public PolicyPurchasedConsumer(ILogger<PolicyPurchasedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PolicyPurchasedEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation(
                "Policy purchased: {PolicyNumber} for Customer {CustomerId}. Coverage: {Coverage}",
                evt.PolicyNumber, evt.CustomerId, evt.CoverageAmount);

            return Task.CompletedTask;
        }
    }
}
