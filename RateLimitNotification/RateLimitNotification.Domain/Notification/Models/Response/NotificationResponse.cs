namespace RateLimitNotification.Domain.Notification.Models.Response
{
    public record NotificationResponse
    {
        public NotificationResponse(string errorMessage)
        {
            this.Message = errorMessage;
            this.HasError = true;
        }

        public NotificationResponse()
        {
            this.Message = "Notification was sent to the user succesfully";
            this.HasError = false;
        }

        public bool HasError { get; set; }
        public string? UserId { get; set; }
        public string? NotificationType { get; set; }
        public string? Message { get; set; }
    }
}
