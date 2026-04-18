using MassTransit;
using Microsoft.Extensions.Logging;
using SmartSure.Contracts.Events;

namespace AdminService.API.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly ILogger<UserRegisteredConsumer> _logger;

        public UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var evt = context.Message;
            _logger.LogInformation(
                "New user registered: {FullName} ({Email}) with role {Role}",
                evt.FullName, evt.Email, evt.Role);

            return Task.CompletedTask;
        }
    }
}
