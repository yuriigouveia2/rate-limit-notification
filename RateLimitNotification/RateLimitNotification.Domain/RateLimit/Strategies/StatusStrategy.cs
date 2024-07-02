using RateLimitNotification.Domain.Abstractions.Configurations;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Domain.RateLimit.Strategies
{
    public class StatusStrategy : INotificationTypeStrategy
    {
        private readonly NotificationTypeConfiguration _notificationTypeConfiguration;

        public StatusStrategy(RateLimitConfiguration rateLimitConfiguration)
        {
            _notificationTypeConfiguration = rateLimitConfiguration.StatusConfiguration;
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
