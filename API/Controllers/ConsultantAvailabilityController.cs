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

        [HttpGet("GetAvailableSlots")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] DateOnly from, [FromQuery] DateOnly to)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var slots = await _availabilityService.GetAvailableSlots(userId, from, to);
            return Ok(slots);
        }

        [HttpPost("CreateSlot")]
        [Authorize(Roles = "2")]
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
