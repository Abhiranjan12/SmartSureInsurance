using AuthService.Application.Services;
using MassTransit;
using SmartSure.Contracts.Events;

namespace AuthService.API.Consumers
{
    public class ClaimRejectedConsumer : IConsumer<ClaimRejectedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ClaimRejectedConsumer> _logger;

        public ClaimRejectedConsumer(IEmailService emailService, ILogger<ClaimRejectedConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ClaimRejectedEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation("Claim {ClaimNumber} rejected. Reason: {Reason}", evt.ClaimNumber, evt.RejectionReason);

            if (!string.IsNullOrEmpty(evt.CustomerEmail))
            {
                string body = "<h2>Hello " + evt.CustomerName + ",</h2>"
                            + "<p>Your claim <strong>" + evt.ClaimNumber + "</strong> has been <strong style='color:red;'>REJECTED</strong>.</p>"
                            + "<p>Reason: " + evt.RejectionReason + "</p>"
                            + "<p>For any queries, please contact support.</p>";

                await _emailService.SendOtpAsync(evt.CustomerEmail, evt.CustomerName, body);
            }
        }
    }
}
