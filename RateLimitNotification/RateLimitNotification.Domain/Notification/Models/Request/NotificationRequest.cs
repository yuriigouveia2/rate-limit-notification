using System.ComponentModel.DataAnnotations;

namespace RateLimitNotification.Domain.Notification.Models.Request
{
    public record NotificationRequest
    {
        [Required]
        public string NotificationType { get; private set; }

        [Required]
        public string UserId { get; private set; }

        [Required]
        public string Message { get; private set; }
    }
}
