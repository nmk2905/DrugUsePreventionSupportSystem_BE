using DTO.UserAssessment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAssessmentController : Controller
    {
        private readonly IUserAssessmentService _userAssessmentService;
        
        public UserAssessmentController(IUserAssessmentService userAssessmentService)
        {
            _userAssessmentService = userAssessmentService;
        }

        [HttpPost("Submit")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Submit([FromBody] SubmitAssessmentRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var result = await _userAssessmentService.SubmitAssessmentAsync(userId, request.AssessmentId, request.SelectedOptionIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
