using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;
using static API.Controllers.AgeGroupController;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class AssessmentController : Controller
    {
        private readonly IAssessmentService _service;
        public AssessmentController(IAssessmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Assessment>>> GetAll()
        {
            var data = await _service.GetAllAssessment();
            var result = data.Select(dto => new AssessmentSampleDto
            {
                AssessmentId = dto.AssessmentId,
                Title = dto.Title,
                Description = dto.Description,
                AssessmentType = dto.AssessmentTypeNavigation?.Name,
                AgeGroup = dto.AgeGroupNavigation?.Name,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            }).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Assessment>> GetById(int id)
        {
            var dto = await _service.GetAssessmentById(id);
            if (dto == null)
                return NotFound(new { message = "Không tìm thấy bài đánh giá." });

            var result = new AssessmentSampleDto
            {
                AssessmentId = dto.AssessmentId,
                Title = dto.Title,
                Description = dto.Description,
                AssessmentType = dto.AssessmentTypeNavigation?.Name,
                AgeGroup = dto.AgeGroupNavigation?.Name,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            };
            return Ok(result);
        }

        //get
        public class AssessmentSampleDto
        {
            public int AssessmentId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string AssessmentType { get; set; }
            public string AgeGroup { get; set; }
            public DateTime? CreatedDate { get; set; }
            public bool? IsActive { get; set; }
        }
    }

} 
