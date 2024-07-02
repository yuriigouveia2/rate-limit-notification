using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace RateLimitNotification.Infra.RateLimit
{
    public class RateLimitCacheRepository : IRateLimitCacheRepository
    {
        private readonly IDatabase _redis;

        public RateLimitCacheRepository(IConnectionMultiplexer muxer)
        {
            _redis = muxer.GetDatabase();
        }

        public async Task<bool> ExistsOnCache(string userId, string notificationType)
        {
            var key = $"user_id:notification_type:{userId}:{notificationType}";
            return await _redis.KeyExistsAsync(key); //TODO: refactor to check if this hash key has reached to the maximum
        }

        public async Task<int> GetNotificationCount(string userId, string notificationType)
        {
            var key = $"user_id:notification_type:{userId}:{notificationType}";
            var notificationCount = await _redis.StringGetAsync(key);

            return notificationCount.HasValue ?
                Convert.ToInt32(notificationCount) : 0;
        }

        public async Task<bool> SaveOrUpdateOnCache(Domain.RateLimit.Entities.RateLimit rateLimit)
        {
            var key = $"user_id:{rateLimit.UserId}";
            var notificationCount = await GetNotificationCount(rateLimit.UserId, rateLimit.NotificationType);

            if (notificationCount == 0)
            {
                return await _redis.StringSetAsync(key, value: 1, expiry: TimeSpan.FromSeconds(2000), When.Always);
            }

            return await _redis.StringSetAsync(key, value: notificationCount + 1, expiry: TimeSpan.FromSeconds(2000));
        }
    }
}
