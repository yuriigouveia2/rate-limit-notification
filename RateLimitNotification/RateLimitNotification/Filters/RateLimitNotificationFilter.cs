using Microsoft.AspNetCore.Mvc.Filters;
using RateLimitNotification.Domain.RateLimit.Interfaces;

namespace RateLimitNotification.Api.Filters
{
    public class RateLimitNotificationFilterAttribute : IAsyncActionFilter
    {
        private readonly IRateLimitService _rateLimitService;

        public RateLimitNotificationFilterAttribute(IRateLimitService rateLimitService)
        {
            _rateLimitService = rateLimitService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {

            }
            catch (Exception)
            {

            }
            finally 
            {
            
            }
        }
    }
}
