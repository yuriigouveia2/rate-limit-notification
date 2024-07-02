namespace RateLimitNotification.Domain.RateLimit.Interfaces
{
    public interface INotificationTypeStrategy
    {
        bool CanNotify(int currentNotificationCount);
        long GetTtl();
    }
}
