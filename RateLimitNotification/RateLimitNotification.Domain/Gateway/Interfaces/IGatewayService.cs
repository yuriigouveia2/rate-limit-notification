using RateLimitNotification.Domain;

namespace RateLimitNotification.Domain.Gateway.Interfaces
{
    public interface IGatewayService
    {
        Task SendNotification(Notification.Entities.Notification notification);
    }
}
