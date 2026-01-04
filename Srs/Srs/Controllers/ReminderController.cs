using Business_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Reminder;

namespace Srs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }
        [HttpPost("SendReminder")]
        public async Task<IActionResult> SendReminder([FromBody] SendReminderDto dto)
        {
            var result = await _reminderService.SendReminderAsync(dto);
            if (result)
            {
                return Ok(new { Message = "Reminder sent successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to send reminder." });
            }
        }
        [HttpGet("GetAllReminders/{EmployeeId}")]
        public async Task<IActionResult> GetAllReminders(int EmployeeId)
        {
            var reminders = await _reminderService.GetAllReminders(EmployeeId);
            return Ok(reminders);
        }
        [HttpPut("MarkReminderRead/{reminderid}")]
        public async Task<IActionResult> MarkReminderRead(int reminderid)
        {
            var result = await _reminderService.MarkAsRead(reminderid);
            if (result)
            {
                return Ok(new { Message = "Reminder marked as read." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to mark reminder as read." });
            }
        }
    }
}
