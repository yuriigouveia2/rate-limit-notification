namespace RateLimitNotification.Domain.Abstractions.Configurations
{
    public class RateLimitConfiguration
    {
        public NotificationTypeConfiguration NewsConfiguration { get; set; }
        public NotificationTypeConfiguration StatusConfiguration { get; set; }
        public NotificationTypeConfiguration MarketingConfiguration { get; set; }
    }
}
