using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;
using static API.Controllers.AgeGroupController;
using static API.Controllers.AssessmentTypeController;

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

        //lấy toàn bộ bài Assessment
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


        //lấy data 1 bài Assessment chỉ định
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

        //add
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AssessmentDto dto)
        {
            var type = new Assessment
            {
                Title = dto.Title,
                Description = dto.Description,
                AssessmentType = dto.AssessmentType,
                AgeGroup = dto.AgeGroup,
                CreatedDate = dto.CreatedDate ?? DateTime.Now, 
                IsActive = dto.IsActive ?? true 
            };

            var id = await _service.AddAssessmentAsync(type);
            return Ok(new { message = "Thêm thành công" });
        }

        //update
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] AssessmentDto dto)
        {
            var existing = await _service.GetAssessmentById(id);
            if (existing == null)
                return NotFound(new { message = "Không tìm thấy." });

            if (dto.Title != null)
                existing.Title = dto.Title;

            if (dto.Description != null)
                existing.Description = dto.Description;

            if (dto.AssessmentType != null)
                existing.AssessmentType = dto.AssessmentType;

            if (dto.AgeGroup != null)
                existing.AgeGroup = dto.AgeGroup;

            if (dto.CreatedDate != null && dto.CreatedDate != default)
                existing.CreatedDate = dto.CreatedDate;

            if (dto.IsActive != null)
                existing.IsActive = dto.IsActive;

            await _service.UpdateAssessmentAsync(existing);
            return Ok(new { message = "Cập nhật thành công" });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAssessmentAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Xóa thành công" });
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

        //add, update
        public class AssessmentDto
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public int? AssessmentType { get; set; }
            public int? AgeGroup { get; set; }
            public DateTime? CreatedDate { get; set; }
            public bool? IsActive { get; set; }
        }
    }

} 
