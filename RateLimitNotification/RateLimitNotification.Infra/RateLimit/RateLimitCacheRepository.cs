using StackExchange.Redis;

namespace RateLimitNotification.Infra.RateLimit
{
    public class RateLimitCacheRepository : IRateLimitCacheRepository
    {
        private readonly IDatabase _redis;

        public RateLimitCacheRepository(IDatabase redis)
        {
            _redis = redis;
        }

        public async Task<bool> ExistsOnCache(string userId, string notificationType)
        {
            var key = $"user_id:{userId}";
            var field = notificationType;
            return await _redis.HashExistsAsync(key, field); //TODO: refactor to check if this hash key and hash video has reached to the maximum
        }

        public async Task<int> GetNotificationCount(string userId, string notificationType)
        {
            var key = $"user_id:{userId}";
            var field = notificationType;
            var notificationCount = await _redis.HashGetAsync(key, field);

            return notificationCount.HasValue ?
                Convert.ToInt32(notificationCount) : 0;
        }

        public async Task<long> SaveOrUpdateOnCache(Domain.RateLimit.Entities.RateLimit rateLimit)
        {
            var key = $"user_id:{rateLimit.UserId}";
            var field = rateLimit.NotificationType;

            return await _redis.HashIncrementAsync(key, field, value: 1, CommandFlags.None);
        }
    }
}
