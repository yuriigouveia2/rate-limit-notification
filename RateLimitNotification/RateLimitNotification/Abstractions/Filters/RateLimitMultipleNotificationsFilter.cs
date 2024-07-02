using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RateLimitNotification.Domain.Notification.Models.Request;
using RateLimitNotification.Domain.Notification.Models.Response;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Api.Abstractions.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RateLimitMultipleNotificationsFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var rateLimitService = context.HttpContext.RequestServices.GetService<IRateLimitService>();
            var notificationsRequest = context.ActionArguments["notificationsRequest"] as ICollection<NotificationRequest> ?? new List<NotificationRequest>(capacity: 0);
            var errorResponse = new List<NotificationResponse>(capacity: notificationsRequest.Count);


            foreach(var request in notificationsRequest) 
            { 
                if (rateLimitService!= null &&
                    request.UserId != null &&
                    request.NotificationType != null &&
                    await rateLimitService.CanNotify(request.UserId, request.NotificationType))
                {
                    continue;
                }
                else
                {
                    errorResponse.Add(new NotificationResponse
                    {
                        HasError = true,
                        ResponseMessage = string.Format("User {0} has reached to maximum number of notifications for Type {1}", request?.UserId, request?.NotificationType),
                        UserId = request?.UserId,
                        NotificationType = request?.NotificationType
                    });
                }
            }

            if (errorResponse.Count > 0)
            {
                context.Result = new BadRequestObjectResult(errorResponse);
            } 
            else
            {
                await next();
            }
        }
    }
}
