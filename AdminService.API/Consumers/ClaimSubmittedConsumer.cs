using MassTransit;
using Microsoft.Extensions.Logging;
using SmartSure.Contracts.Events;

namespace AdminService.API.Consumers
{
    public class ClaimSubmittedConsumer : IConsumer<ClaimSubmittedEvent>
    {
        private readonly ILogger<ClaimSubmittedConsumer> _logger;

        public ClaimSubmittedConsumer(ILogger<ClaimSubmittedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ClaimSubmittedEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation(
                "New claim submitted: {ClaimNumber} by Customer {CustomerId}. Amount: {Amount}",
                evt.ClaimNumber, evt.CustomerId, evt.ClaimAmount);

            return Task.CompletedTask;
        }
    }
}
