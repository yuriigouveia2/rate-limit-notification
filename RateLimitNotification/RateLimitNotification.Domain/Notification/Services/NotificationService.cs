using Microsoft.Extensions.Logging;
using RateLimitNotification.Domain.Gateway.Interfaces;
using RateLimitNotification.Domain.Notification.Interfaces;
using RateLimitNotification.Domain.Notification.Models.Request;
using RateLimitNotification.Domain.Notification.Models.Response;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Domain.Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IRateLimitService _rateLimitService;
        private readonly IGatewayService _gateway;
        private readonly INotificationTypeStrategyResolver _strategyResolver;

        public NotificationService(
            ILogger<NotificationService> logger,
            IGatewayService gateway,
            IRateLimitService rateLimitService,
            INotificationTypeStrategyResolver strategyResolver)
        {
            _logger = logger;
            _gateway = gateway;
            _rateLimitService = rateLimitService;
            _strategyResolver = strategyResolver;
        }

        public async Task<NotificationResponse> Send(NotificationRequest notificationRequest)
        {
            try
            {
                var notification = new Entities.Notification(
                    notificationRequest.NotificationType,
                    notificationRequest.UserId,
                    notificationRequest.Message);

                var notificationSent = await SendNotification(notification);

                if (notificationSent)
                {
                    return new NotificationResponse(notificationRequest.UserId, notificationRequest.NotificationType);
                }

                return new NotificationResponse(notificationRequest.UserId, notificationRequest.NotificationType, errorMessage: "Notification could not be sent");
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception was raised while sending a notification to the user");
                return new NotificationResponse(notificationRequest.UserId, notificationRequest.NotificationType, errorMessage: ex.Message);
            }
        }

        public async Task<IEnumerable<NotificationResponse>> SendMultiple(ICollection<NotificationRequest> notificationsRequest)
        {
            ICollection<NotificationResponse> response = new List<NotificationResponse>(capacity: notificationsRequest.Count);

            foreach (var request in notificationsRequest)
            {
                try
                {
                    var notification = new Entities.Notification(
                        request.NotificationType,
                        request.UserId,
                        request.Message);

                    var notificationSent = await SendNotification(notification);

                    if (notificationSent)
                    {
                        response.Add(new NotificationResponse(request.UserId, request.NotificationType));
                    }
                    else
                    {
                        response.Add(new NotificationResponse(request.UserId, request.NotificationType, errorMessage: "Notification could not be sent"));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("An exception was raised while sending a notification to the user");
                    response.Add(new NotificationResponse(request.UserId, request.NotificationType, errorMessage: ex.Message));
                }
            }

            return response;
        }

        #region Private methods

        private async Task<bool> SendNotification(Entities.Notification notification)
        {
            var strategy = _strategyResolver.Get(notification.NotificationType);
            var timeToLive = strategy.GetTtl();

            var rateLimit = new RateLimit.Entities.RateLimit(notification.UserId, notification.NotificationType, timeToLive);
            var saved = await _rateLimitService.SaveOnCache(rateLimit);

            if (saved)
            {
                await _gateway.SendNotification(notification);
            }

            return saved;
        }

        #endregion
    }
}
