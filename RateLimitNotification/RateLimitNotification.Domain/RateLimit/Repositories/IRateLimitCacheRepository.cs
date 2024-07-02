namespace RateLimitNotification.Infra.RateLimit
{
    public interface IRateLimitCacheRepository
    {
        Task<int> GetNotificationCount(string userId, string notificationType);
        Task<bool> SaveOnCache(Domain.RateLimit.Entities.RateLimit rateLimit);
    }
}
