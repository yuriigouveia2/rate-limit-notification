using Microsoft.Extensions.Logging;
using RateLimitNotification.Domain.Gateway.Interfaces;
using RateLimitNotification.Domain.Notification.Interfaces;
using RateLimitNotification.Domain.Notification.Models.Request;
using RateLimitNotification.Domain.Notification.Models.Response;

namespace RateLimitNotification.Domain.Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IGatewayService _gateway;

        public NotificationService(ILogger<NotificationService> logger, IGatewayService gateway)
        {
            _logger = logger;
            _gateway = gateway;
        }

        public async Task<NotificationResponse> Send(NotificationRequest notificationRequest)
        {
            try
            {
                var notification = new Entities.Notification(
                    notificationRequest.NotificationType,
                    notificationRequest.UserId,
                    notificationRequest.Message);

                await _gateway.SendNotification(notification);

                return new NotificationResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception was raised while sending a notification to the user");
                return new NotificationResponse(errorMessage: ex.Message);
            }

        }
    }
}
