using System.ComponentModel.DataAnnotations;

namespace RateLimitNotification.Domain.Notification.Models.Request
{
    /// <summary>
    /// A notification
    /// </summary>
    public class NotificationRequest
    {
        /// <summary>
        /// Notification Type
        /// </summary>
        /// <example>marketing</example>
        [Required(ErrorMessage = "NotificationType is required")]
        public string NotificationType { get; set; } = string.Empty;

        /// <summary>
        /// User identification
        /// </summary>
        /// <example>15</example>
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Notification message
        /// </summary>
        /// <example>We have a new product which should be relevant to you.</example>
        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; } = string.Empty;
    }
}
