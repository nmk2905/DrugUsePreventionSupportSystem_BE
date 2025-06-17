using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultantController : Controller
    {
        private readonly IConsultantService _consultantService;

        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [HttpGet("All-Consultants")]
        [Authorize(Roles = "1")] //admin
        public async Task<IActionResult> GetAllConsultants()
        {
            var consultants = await _consultantService.GetAllConsultant();
            return Ok(consultants);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1,2")] // Admin, Cons
        public async Task<IActionResult> GetConsultantById(int id)
        {
            var consultant = await _consultantService.GetConsultantById(id);
            return consultant == null ? NotFound() : Ok(consultant);
        }

        [HttpPut("Update-profile")]
        [Authorize(Roles = "2")] 
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateConsultantRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var consultant = await _consultantService.GetAllConsultant();
            var current = consultant.FirstOrDefault(c => c.ConsultantNavigation?.UserId == userId);

            if (current == null)
                return BadRequest("Bạn không phải là tư vấn viên hợp lệ.");

            current.Specification = request.Specification;
            current.Qualifications = request.Qualifications;
            current.ExperienceYears = request.ExperienceYears;

            var updated = await _consultantService.UpdateProfileAsync(current);
            return Ok(updated);
        }

        [HttpGet("Available")]
        [Authorize(Roles = "3")] // Customer gọi lấy các active cons
        public async Task<IActionResult> GetAvailableConsultants()
        {
            var consultants = await _consultantService.GetActiveConsultantsAsync();

            var result = consultants.Select(c => new ConsultantPreview
            {
                ConsultantId = c.ConsultantId,
                FullName = c.ConsultantNavigation?.FullName,
                Specification = c.Specification,
                ExperienceYears = c.ExperienceYears
            });

            return Ok(result);
        }
    }



    public class UpdateConsultantRequest
    {
        public string Specification { get; set; }
        public string Qualifications { get; set; }
        public int ExperienceYears { get; set; }
    }

    public class ConsultantPreview
    {
        public int ConsultantId { get; set; }
        public string FullName { get; set; }
        public string Specification { get; set; }
        public int ExperienceYears { get; set; }
    }


}
