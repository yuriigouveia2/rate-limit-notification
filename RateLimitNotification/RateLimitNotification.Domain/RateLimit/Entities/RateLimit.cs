namespace RateLimitNotification.Domain.RateLimit.Entities
{
    public class RateLimit
    {
        public RateLimit(string userId, string notificationType) 
        {
            this.UserId = userId;
            this.NotificationType = notificationType;
        }

        public string UserId { get; private set; }
        public string NotificationType { get; private set; }
    }
}
