namespace RateLimitNotification.Domain.RateLimit.Interfaces
{
    public interface INotificationTypeStrategyResolver
    {
        INotificationTypeStrategy Get(string notificationType);
    }
}
