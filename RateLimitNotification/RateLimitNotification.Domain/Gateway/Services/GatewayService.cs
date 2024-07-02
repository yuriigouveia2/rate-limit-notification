using Microsoft.Extensions.Logging;
using RateLimitNotification.Domain.Gateway.Interfaces;

namespace RateLimitNotification.Domain.Gateway.Services
{
    public class GatewayService : IGatewayService
    {
        private ILogger<GatewayService> _logger;
        public GatewayService(ILogger<GatewayService> logger)
        {
            _logger = logger;
        }

        public async Task SendNotification(Notification.Entities.Notification notification)
        {
            await Task.FromResult(() =>
            {
                var message = string.Format("Sending message to user {0}", notification.UserId);

                _logger.LogInformation(message);
                Console.WriteLine(message);
            });
        }


    }
}
