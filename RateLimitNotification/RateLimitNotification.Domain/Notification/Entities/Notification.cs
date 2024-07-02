namespace RateLimitNotification.Domain.Notification.Entities
{
    public class Notification
    {
        protected Notification() { }

        public Notification(string notificationType, string userId, string message)
        {
            this.NotificationType = notificationType;
            this.UserId = userId;
            this.Message = message;
        }

        public string? NotificationType { get; private set; }
        public string? UserId { get; private set; }
        public string? Message { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }

    }
}
