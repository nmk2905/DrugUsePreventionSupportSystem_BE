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
        private readonly IUserAssessmentService _service;
        
        public UserAssessmentController(IUserAssessmentService userAssessmentService)
        {
            _service = userAssessmentService;
        }

        //submit bài Assessment
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitAssessmentDto dto)
        {
            var result = await _service.SubmitAssessmentAsync(
                dto.UserId,
                dto.AssessmentId,
                dto.SelectedOptionIds
            );

            return Ok(new
            {
                Score = result.Score,
                RiskLevel = result.RiskLevelNavigation?.RiskLevel1,
                Description = result.RiskLevelNavigation?.RiskDescription
            });
        }



        public class SubmitAssessmentDto
        {
            public int UserId { get; set; }
            public int AssessmentId { get; set; }
            public List<int> SelectedOptionIds { get; set; }
        }


    }
}
