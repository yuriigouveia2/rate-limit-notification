using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RateLimitNotification.Api.Abstractions.Filters;
using RateLimitNotification.Domain.Notification.Interfaces;
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
        private readonly INotificationService _notificationService;

        public NotificationsController(ILogger<NotificationsController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
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
        [RateLimitSingleNotificationFilter]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendNotification(
            [FromBody] NotificationRequest notificationRequest)
        {
            try
            {            
                _logger.LogInformation("Received single notification request");
                if (!TryValidateModel(notificationRequest))
                {
                    return BadRequest(ModelState);
                }

                var response = await _notificationService.Send(notificationRequest);

                return StatusCode((int)HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception was raised while handling the request");
                return BadRequest(new NotificationResponse(notificationRequest.UserId, notificationRequest.NotificationType, ex.Message));
            }
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
        [RateLimitMultipleNotificationsFilter]
        [HttpPost("multiple")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> SendNotification(
            [FromBody] ICollection<NotificationRequest> notificationsRequest)
        {
            try
            {
                _logger.LogInformation("Received multiple notification request");
                if (!TryValidateModel(notificationsRequest))
                {
                    return BadRequest(ModelState);
                }

                var response = await _notificationService.SendMultiple(notificationsRequest);

                return StatusCode((int)HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception was raised while handling the request");
                
                return BadRequest(
                    new List<NotificationResponse>(1) 
                    { 
                        new NotificationResponse(ex.Message) 
                    });
            }
        }
    }
}
