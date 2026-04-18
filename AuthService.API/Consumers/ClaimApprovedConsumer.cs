using AuthService.Application.Services;
using MassTransit;
using SmartSure.Contracts.Events;

namespace AuthService.API.Consumers
{
    public class ClaimApprovedConsumer : IConsumer<ClaimApprovedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ClaimApprovedConsumer> _logger;

        public ClaimApprovedConsumer(IEmailService emailService, ILogger<ClaimApprovedConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ClaimApprovedEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation("Claim {ClaimNumber} approved. Amount: {Amount}", evt.ClaimNumber, evt.ApprovedAmount);

            if (!string.IsNullOrEmpty(evt.CustomerEmail))
            {
                string body = "<h2>Hello " + evt.CustomerName + ",</h2>"
                            + "<p>Your claim <strong>" + evt.ClaimNumber + "</strong> has been <strong style='color:green;'>APPROVED</strong>.</p>"
                            + "<p>Approved Amount: <strong>" + evt.ApprovedAmount + "</strong></p>"
                            + "<p>Thank you for choosing SmartSure.</p>";

                await _emailService.SendOtpAsync(evt.CustomerEmail, evt.CustomerName, body);
            }
        }
    }
}
