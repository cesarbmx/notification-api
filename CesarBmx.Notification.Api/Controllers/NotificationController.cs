using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CesarBmx.Shared.Application.Responses;
using CesarBmx.Notification.Application.Responses;
using CesarBmx.Notification.Application.Services;
using CesarBmx.Shared.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using CesarBmx.Notification.Application.Requests;

namespace CesarBmx.Notification.Api.Controllers
{
    [SwaggerResponse(500, Type = typeof(InternalServerError))]
    [SwaggerResponse(401, Type = typeof(Unauthorized))]
    [SwaggerResponse(403, Type = typeof(Forbidden))]
    [SwaggerOrder(orderPrefix: "G")]
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get notifications
        /// </summary>
        [HttpGet]
        [Route("api/notifications")]
        [SwaggerResponse(200, Type = typeof(List<Application.Responses.Notification>))]
        [SwaggerOperation(Tags = new[] { "Notifications" }, OperationId = "Notifications_Getnotifications")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            // Reponse
            var response = await _notificationService.GetNotifications(userId);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Get message
        /// </summary>
        [HttpGet]
        [Route("api/notifications/{messageId}", Name = "Notifications_GetNotification")]
        [SwaggerResponse(200, Type = typeof(Application.Responses.Notification))]
        [SwaggerResponse(404, Type = typeof(NotFound))]
        [SwaggerOperation(Tags = new[] { "Notifications" }, OperationId = "Notifications_GetNotification")]
        public async Task<IActionResult> GetNotification(Guid notificationId)
        {
            // Reponse
            var response = await _notificationService.GetNotification(notificationId);

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Create notification
        /// </summary>
        [HttpPost]
        [Route("api/notifications")]
        [SwaggerResponse(201, Type = typeof(Application.Responses.Notification))]
        [SwaggerResponse(400, Type = typeof(BadRequest))]
        //[SwaggerResponse(409, Type = typeof(Conflict<CreateNotificationConflict>))]
        [SwaggerResponse(422, Type = typeof(ValidationFailed))]
        [SwaggerOperation(Tags = new[] { "Notifications" }, OperationId = "Notifications_CreateNotification")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotification request)
        {
            // Reponse
            var response = await _notificationService.CreateNotification(request);

            // Return
            return CreatedAtRoute("Notifications_GetNotification", new { response.NotificationId }, response);
        }
    }
}

