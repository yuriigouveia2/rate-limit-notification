using RateLimitNotification.Domain.Abstractions.Configurations;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Domain.RateLimit.Strategies
{
    public class MarketingStrategy : INotificationTypeStrategy
    {
        private readonly NotificationTypeConfiguration _notificationTypeConfiguration;

        public MarketingStrategy(RateLimitConfiguration rateLimitConfiguration)
        {
            _notificationTypeConfiguration = rateLimitConfiguration.MarketingConfiguration;
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
