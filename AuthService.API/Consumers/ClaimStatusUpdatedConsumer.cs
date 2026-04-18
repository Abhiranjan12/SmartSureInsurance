using AuthService.Application.Services;
using MassTransit;
using SmartSure.Contracts.Events;

namespace AuthService.API.Consumers
{
    public class ClaimStatusUpdatedConsumer : IConsumer<ClaimStatusUpdatedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ClaimStatusUpdatedConsumer> _logger;

        public ClaimStatusUpdatedConsumer(IEmailService emailService, ILogger<ClaimStatusUpdatedConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ClaimStatusUpdatedEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation("Claim {ClaimNumber} status updated to {Status}", evt.ClaimNumber, evt.NewStatus);
            string body = "<h2>Hello " + evt.CustomerName + ",</h2>"
                        + "<p>Your claim <strong>" + evt.ClaimNumber + "</strong> has been updated.</p>"
                        + "<p>New Status: <strong>" + evt.NewStatus + "</strong></p>";

            if (evt.NewStatus == "Rejected" && !string.IsNullOrEmpty(evt.RejectionReason))
                body += "<p>Reason: " + evt.RejectionReason + "</p>";

            body += "<p>Thank you for choosing SmartSure.</p>";

            await _emailService.SendOtpAsync(evt.CustomerEmail, evt.CustomerName, body);
        }
    }
}
