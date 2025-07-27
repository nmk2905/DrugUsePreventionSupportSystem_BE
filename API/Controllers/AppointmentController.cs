using DTO.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Enums.Appointment;
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
        [Authorize(Roles = "3")] // user book lịch 1 cons chỉ định
        public async Task<IActionResult> Book([FromBody] BookAppointmentRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var appointment = await _appointmentService.BookAppointmentAsync(userId, request.AvailabilityId);

                var dto = new AppointmentDto
                {
                    AppointmentId = appointment.AppointmentId,
                    Status = appointment.Status,
                    CreatedDate = appointment.CreatedDate,
                    MeetingLink = appointment.MeetingLink,
                    MaterialId = appointment.Material,
                    ConsultantName = appointment.Consultant?.ConsultantNavigation?.FullName,
                    UserName = appointment.User?.FullName
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi tạo appointment: " + (ex.InnerException?.Message ?? ex.Message) });
            }
        }

        //xem các lịch đã được book của customer và cons
        [HttpGet("MyAppointments")]
        [Authorize(Roles = "2,3")]
        public async Task<IActionResult> MyAppointments()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);

            var result = appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                ConsultantName = a.Consultant?.ConsultantNavigation?.FullName,
                UserName = a.User?.FullName,
                Status = a.Status,
                CreatedDate = a.CreatedDate,
                MeetingLink = a.MeetingLink,
                MaterialId = a.Material
            }).ToList();

            return Ok(result);
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "3")] // User hủy lịch, chỉ áp dụng với lịch ở status là pending
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _appointmentService.CancelAppointmentAsync(id, userId);
            return success ? Ok(new { message = "Hủy lịch thành công" }) : NotFound();
        }



        [HttpPut("Approve-appointment")]
        [Authorize(Roles = "1")] // Admin duyệt appointment
        public async Task<IActionResult> ApproveAppointment([FromBody] ApproveAppointmentRequest request)
        {
            var success = await _appointmentService.UpdateStatusAsync(request.AppointmentId, AppointmentStatus.Confirmed, request.MeetingLink);
            return success ? Ok(new { message = "Lịch đã được duyệt và gửi link thành công." }) : NotFound();
        }
    }

    

    

}
