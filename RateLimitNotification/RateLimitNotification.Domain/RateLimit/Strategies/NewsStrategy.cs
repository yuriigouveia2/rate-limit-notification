using RateLimitNotification.Domain.Abstractions.Configurations;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Domain.RateLimit.Strategies
{
    public class NewsStrategy : INotificationTypeStrategy
    {
        private readonly NotificationTypeConfiguration _notificationTypeConfiguration;

        public NewsStrategy(RateLimitConfiguration rateLimitConfiguration)
        {
            _notificationTypeConfiguration = rateLimitConfiguration.NewsConfiguration;
        }

        public bool CanNotify(int currentNotificationCount)
        {
            return _notificationTypeConfiguration.MaxAmount > currentNotificationCount;
        }

        public long GetTtl()
        {
            return _notificationTypeConfiguration.ExpiryTimeInMin;
        }
    }
}
