using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RateLimitNotification.Domain.Notification.Models.Request;
using RateLimitNotification.Domain.Notification.Models.Response;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Api.Abstractions.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RateLimitSingleNotificationFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var rateLimitService = context.HttpContext.RequestServices.GetService<IRateLimitService>();
            var notificationRequest = context.ActionArguments["notificationRequest"] as NotificationRequest;

            if (rateLimitService!= null && 
                notificationRequest != null &&
                notificationRequest.UserId != null &&
                notificationRequest.NotificationType != null &&
                await rateLimitService.CanNotify(notificationRequest.UserId, notificationRequest.NotificationType))
            {
                await next();
            }
            else
            {
                context.Result = new BadRequestObjectResult(new NotificationResponse
                {
                    HasError = true,
                    ResponseMessage = string.Format("User {0} has reached to maximum number of notifications for Type {1}", notificationRequest?.UserId, notificationRequest?.NotificationType),
                    UserId = notificationRequest?.UserId,
                    NotificationType = notificationRequest?.NotificationType
                });
            }
        }
    }
}
