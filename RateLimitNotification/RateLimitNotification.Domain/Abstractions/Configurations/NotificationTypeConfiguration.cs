namespace RateLimitNotification.Domain.Abstractions.Configurations
{
    public class NotificationTypeConfiguration
    {
        public NotificationTypeConfiguration() { }

        public int MaxAmount { get; set; }
        public int ExpiryTimeInMin { get; set; }
    }
}
