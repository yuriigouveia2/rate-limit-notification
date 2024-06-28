namespace RateLimitNotification.Domain.RateLimit.Interfaces
{
    public interface IRateLimitService
    {
        Task<bool> CanNotify(string userId, string notificationType);
    }
}
