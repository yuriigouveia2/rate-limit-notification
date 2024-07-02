using Microsoft.Extensions.Logging;
using RateLimitNotification.Domain.Abstractions.Configurations;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Domain.RateLimit.Strategies
{
    public class NotificationTypeStrategyResolver : INotificationTypeStrategyResolver
    {
        private readonly ILogger<NotificationTypeStrategyResolver> _logger;
        private readonly RateLimitConfiguration _rateLimitConfiguration;

        public NotificationTypeStrategyResolver(
            ILogger<NotificationTypeStrategyResolver> logger,
            RateLimitConfiguration rateLimitConfiguration)
        {
            _logger = logger;
            _rateLimitConfiguration = rateLimitConfiguration;
        }


        public INotificationTypeStrategy Get(string notificationType)
        {
            return notificationType switch
            {
                "news" => new NewsStrategy(_rateLimitConfiguration),
                "status" => new StatusStrategy(_rateLimitConfiguration),
                "marketing" => new MarketingStrategy(_rateLimitConfiguration),
                _ => throw new NotSupportedException()
            };
        }
    }
}
