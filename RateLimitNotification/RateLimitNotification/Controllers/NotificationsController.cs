using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RateLimitNotification.Api.Abstractions.Filters;
using RateLimitNotification.Domain.Notification.Models.Request;
using RateLimitNotification.Domain.Notification.Models.Response;
using System.Net;

namespace RateLimitNotification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Creates a Notification.
        /// </summary>
        /// <param name="notificationRequest"></param>
        /// <returns>A newly created Notification</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /
        ///     {
        ///        "userId": "15",
        ///        "notificationType": "marketing",
        ///        "message": "We have a new product which should be relevant to you."
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created notification</response>
        /// <response code="400">If an error is raised</response>
        [RateLimitSingleNotificationFilterAttribute]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendNotification(
            [FromBody] NotificationRequest notificationRequest)
        {
            if (!TryValidateModel(notificationRequest))
            {
                return BadRequest(ModelState);
            }

            await Task.Run(() => { });

            return StatusCode((int)HttpStatusCode.Created, new NotificationResponse());
        }

        /// <summary>
        /// Creates multiple Notifications.
        /// </summary>
        /// <param name="notificationsRequest"></param>
        /// <returns>The newly created Notification(s)</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /multiple
        ///     [
        ///         {
        ///             "userId": "15",
        ///             "notificationType": "marketing",
        ///             "message": "We have a new product which should be relevant to you."
        ///         },
        ///         {
        ///             "userId": "252",
        ///             "notificationType": "status",
        ///             "message": "The payment of your new product is peding."
        ///         },
        ///     ]
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created notification</response>
        /// <response code="400">If an error is raised</response>
        [HttpPost("multiple")]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> SendNotification(
            [FromBody] ICollection<NotificationRequest> notificationsRequest)
        {
            if (!TryValidateModel(notificationsRequest))
            {
                return BadRequest(ModelState);
            }

            return StatusCode((int)HttpStatusCode.Created, new List<NotificationResponse>());
        }
    }
}
