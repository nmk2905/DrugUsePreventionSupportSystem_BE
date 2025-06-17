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
        [Authorize(Roles = "3")] // dành cho customer - lấy lịch rảnh của cons chỉ định
        public async Task<IActionResult> GetAvailableSlots(
        [FromQuery] int consultantId,
        [FromQuery] string from,
        [FromQuery] string to)
        {
            if (!DateOnly.TryParse(from, out var fromDate) || !DateOnly.TryParse(to, out var toDate))
                return BadRequest("from/to không hợp lệ. Định dạng đúng: yyyy-MM-dd");

            var slots = await _availabilityService.GetAvailableSlots(consultantId, fromDate, toDate);
            return Ok(slots);
        }


        [HttpGet("GetAvailableSlots")]
        [Authorize(Roles = "2")] //lấy lịch trống của cons -  dành cho cons
        public async Task<IActionResult> GetAvailableSlots([FromQuery] string from, [FromQuery] string to)
        {
            var fromDate = DateOnly.Parse(from);
            var toDate = DateOnly.Parse(to);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var slots = await _availabilityService.GetAvailableSlots(userId, fromDate, toDate);
            return Ok(slots);
        }


        [HttpPost("CreateSlot")]
        [Authorize(Roles = "2")] //cons
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _availabilityService.CreateSlot(userId, request.SpecificDate, request.StartTime, request.EndTime);
            return Ok(result);
        }

        public class CreateSlotRequest
        {
            public DateOnly SpecificDate { get; set; }
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
        }

    }
}
