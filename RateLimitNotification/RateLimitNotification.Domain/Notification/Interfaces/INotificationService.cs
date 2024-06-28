using RateLimitNotification.Domain.Notification.Models.Request;
using RateLimitNotification.Domain.Notification.Models.Response;

namespace RateLimitNotification.Domain.Notification.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationResponse> Send(NotificationRequest notificationRequest);
    }
}
