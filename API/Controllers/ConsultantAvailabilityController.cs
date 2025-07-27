using DTO.ConsultantAvailability;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultantAvailabilityController : Controller
    {
        private readonly IAvailabilityService _availabilityService;

        public ConsultantAvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet("AvailableSlots")]
        [Authorize(Roles = "3")] // user xem được các slot trống của 1 consultant
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] int consultantId,
            [FromQuery] string from,
            [FromQuery] string to)
        {
            if (!DateOnly.TryParse(from, out var fromDate) || !DateOnly.TryParse(to, out var toDate))
                return BadRequest("from/to không hợp lệ. Định dạng đúng: yyyy-MM-dd");

            var slots = await _availabilityService.GetAvailableSlots(consultantId, fromDate, toDate);

            var result = slots.Select(s => new SlotDto
            {
                AvailabilityId = s.AvailabilityId,
                SpecificDate = s.SpecificDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                ConsultantName = s.Consultant?.ConsultantNavigation?.FullName ?? "(Không rõ)"
            });

            return Ok(result);
        }



        [HttpGet("GetAvailableSlots")]
        [Authorize(Roles = "2")] // tư vấn viên
        public async Task<IActionResult> GetAvailableSlots([FromQuery] string from, [FromQuery] string to)
        {
            if (!DateOnly.TryParse(from, out var fromDate) || !DateOnly.TryParse(to, out var toDate))
                return BadRequest("from/to không hợp lệ");

            var consultantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var slots = await _availabilityService.GetAvailableSlotsWithConsultant(consultantId, fromDate, toDate);

            var result = slots.Select(s => new ConsultantSlotDto
            {
                AvailabilityId = s.AvailabilityId,
                ConsultantId = s.ConsultantId,
                SpecificDate = s.SpecificDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsAvailable = s.IsAvailable,
                Consultant = s.Consultant?.ConsultantNavigation?.FullName ?? "(Không rõ)"
            });

            return Ok(result);
        }


        //consultant đăng kí slot làm việc
        [HttpPost("CreateSlot")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequest request)
        {
            var consultantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var slot = await _availabilityService.CreateSlot(consultantId, request.SpecificDate, request.StartTime, request.EndTime);

            var fullSlot = await _availabilityService.GetByIdWithConsultantAsync(slot.AvailabilityId);

            var result = new SlotDto
            {
                AvailabilityId = fullSlot.AvailabilityId,
                SpecificDate = fullSlot.SpecificDate,
                StartTime = fullSlot.StartTime,
                EndTime = fullSlot.EndTime,
                ConsultantName = fullSlot.Consultant?.ConsultantNavigation?.FullName ?? "(Không rõ)"
            };

            return Ok(result);
        }



        

        



    }
}
