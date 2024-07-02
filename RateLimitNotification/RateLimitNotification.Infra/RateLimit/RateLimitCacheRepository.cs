using Newtonsoft.Json.Linq;
using RateLimitNotification.Domain.RateLimit.Entities;
using StackExchange.Redis;

namespace RateLimitNotification.Infra.RateLimit
{
    public class RateLimitCacheRepository : IRateLimitCacheRepository
    {
        private readonly IDatabase _redis;
        private readonly IServer _server;

        public RateLimitCacheRepository(IConnectionMultiplexer muxer)
        {
            _redis = muxer.GetDatabase();
            _server = muxer.GetServer(muxer.GetEndPoints().First());
        }

        public async Task<int> GetNotificationCount(string userId, string notificationType)
        {
            var pattern = $"user_id:notification_type:timestamp:{userId}:{notificationType}*";
            var notificationsKeys = await Task.Run(() => _server.Keys(database: _redis.Database, pattern: pattern).ToArray());

            return notificationsKeys.Length;
        }

        public async Task<bool> SaveOnCache(Domain.RateLimit.Entities.RateLimit rateLimit)
        {
            var key = $"user_id:notification_type:timestamp:{rateLimit.UserId}:{rateLimit.NotificationType}:{DateTime.Now.Ticks}";

            return await _redis.StringSetAsync(key, 1, expiry: rateLimit.Ttl);
        }
    }
}
