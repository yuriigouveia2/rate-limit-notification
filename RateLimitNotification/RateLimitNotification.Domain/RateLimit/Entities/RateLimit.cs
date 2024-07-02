namespace RateLimitNotification.Domain.RateLimit.Entities
{
    public class RateLimit
    {
        public RateLimit(string userId, string notificationType, long expiryInMin) 
        {
            this.UserId = userId;
            this.NotificationType = notificationType;
            this.Ttl = TimeSpan.FromMinutes(expiryInMin);
        }

        public string UserId { get; private set; }
        public string NotificationType { get; private set; }
        public TimeSpan Ttl { get; private set; }
    }
}
