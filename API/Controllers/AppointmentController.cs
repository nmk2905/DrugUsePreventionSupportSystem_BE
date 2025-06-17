using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("Book")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Book([FromBody] BookAppointmentRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                var appointment = await _appointmentService.BookAppointmentAsync(userId, request.AvailabilityId);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("MyAppointments")]
        [Authorize(Roles = "2,3")] //cons, customer
        public async Task<IActionResult> MyAppointments()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "3")] //cons
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _appointmentService.CancelAppointmentAsync(id, userId);
            return success ? Ok(new { message = "Hủy thành công" }) : NotFound();
        }

        [HttpPut("UpdateStatus")]
        [Authorize(Roles = "1,2")] // admin, cons
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequest request)
        {
            var success = await _appointmentService.UpdateStatusAsync(request.AppointmentId, request.Status);
            return success ? Ok(new { message = "Cập nhật thành công" }) : NotFound();
        }
    }

    public class BookAppointmentRequest
    {
        public int AvailabilityId { get; set; }
    }

    public class UpdateStatusRequest
    {
        public int AppointmentId { get; set; }
        public string Status { get; set; }
    }
}
