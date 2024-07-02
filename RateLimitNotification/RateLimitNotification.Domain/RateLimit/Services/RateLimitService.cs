using Microsoft.Extensions.Logging;
using RateLimitNotification.Domain.RateLimit.Interfaces;
using RateLimitNotification.Infra.RateLimit;

namespace RateLimitNotification.Domain.RateLimit.Services
{
    public class RateLimitService : IRateLimitService
    {
        private readonly ILogger<RateLimitService> _logger;
        private readonly IRateLimitCacheRepository _rateLimitCacheRepository;
        private readonly INotificationTypeStrategyResolver _strategyResolver;

        public RateLimitService(
            ILogger<RateLimitService> logger, 
            IRateLimitCacheRepository rateLimitCacheRepository, 
            INotificationTypeStrategyResolver strategyResolver)
        {
            _logger = logger;
            _rateLimitCacheRepository = rateLimitCacheRepository;
            _strategyResolver = strategyResolver;
        }

        public async Task<bool> CanNotify(string userId, string notificationType)
        {
            try
            {
                _logger.LogInformation("Checking if user could be notified based on cache data");
                var notificationCount = await _rateLimitCacheRepository.GetNotificationCount(userId, notificationType);
                var strategy = _strategyResolver.Get(notificationType);

                return strategy.CanNotify(currentNotificationCount: notificationCount);
            }
            catch (Exception)
            {
                _logger.LogError("An exception was raised while checking if user could be notified based on cache data");
                return false;
            }
        }

        public async Task<bool> SaveOnCache(Domain.RateLimit.Entities.RateLimit rateLimit)
        {
            try
            {
                _logger.LogInformation("Saving notification data on cache");
                return await _rateLimitCacheRepository.SaveOnCache(rateLimit);
            }
            catch (Exception)
            {
                _logger.LogError("An exception was raised while saving notification data on cache");
                return false;
            }
        }
    }
}
