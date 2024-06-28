using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RateLimitNotification.Domain.Notification.Models.Request;
using RateLimitNotification.Domain.Notification.Models.Response;

namespace RateLimitNotification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponse>> SendNotification([FromBody] NotificationRequest notification)
        {
            return Ok(new NotificationResponse());
        }
    }
}
