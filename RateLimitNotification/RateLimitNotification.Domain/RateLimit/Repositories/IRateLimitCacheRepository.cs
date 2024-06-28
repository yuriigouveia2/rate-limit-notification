namespace RateLimitNotification.Infra.RateLimit
{
    public interface IRateLimitCacheRepository
    {
        Task<bool> ExistsOnCache(string userId, string notificationType);
        Task<int> GetNotificationCount(string userId, string notificationType);
        Task<long> SaveOrUpdateOnCache(Domain.RateLimit.Entities.RateLimit rateLimit);
    }
}
