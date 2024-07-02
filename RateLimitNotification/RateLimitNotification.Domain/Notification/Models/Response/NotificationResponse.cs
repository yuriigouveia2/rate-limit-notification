namespace RateLimitNotification.Domain.Notification.Models.Response
{
    public record NotificationResponse
    {
        public NotificationResponse()
        {

        }
        public NotificationResponse(string errorMessage)
        {
            this.ResponseMessage = errorMessage;
            this.HasError = true;
        }

        public NotificationResponse(string userId, string notificationType, string errorMessage)
        {
            this.UserId = userId;
            this.NotificationType = notificationType;
            this.ResponseMessage = errorMessage;
            this.HasError = true;
        }

        public NotificationResponse(string userId, string notificationType)
        {
            this.ResponseMessage = "Notification was sent to the user succesfully";
            this.HasError = false;
            this.UserId = userId;
            this.NotificationType = notificationType;
        }

        public bool HasError { get; set; }
        public string? UserId { get; set; }
        public string? NotificationType { get; set; }
        public string? ResponseMessage { get; set; }
    }
}
