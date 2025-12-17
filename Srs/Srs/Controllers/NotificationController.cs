using Business_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Notifications;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotification _notificationService;
        public NotificationController(INotification notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto dto)
        {
            var result = await _notificationService.SendNotificationAsync(dto);
            if (result)
            {
                return Ok(new { message = "Notification sent successfully." });
            }
            return StatusCode(500, new { message = "Failed to send notification." });
        }
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetNotifications(int employeeId)
        {
            var notifications = await _notificationService.GetAllNotification(employeeId);
            return Ok(notifications);
        }
    }
}
