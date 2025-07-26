using DTO.Assessment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("full")]
        public async Task<IActionResult> CreateFullAssessment([FromBody] CreateAssessmentDto dto)
        {
            try
            {
                // Validate cơ bản
                if (dto == null || string.IsNullOrWhiteSpace(dto.Title))
                    return BadRequest("Assessment title is required.");

                if (dto.Questions == null || !dto.Questions.Any())
                    return BadRequest("At least one question is required.");

                // Tạo danh sách câu hỏi
                var questions = dto.Questions.Select(q =>
                {
                    if (string.IsNullOrWhiteSpace(q.QuestionText))
                        throw new ArgumentException("Question text is required.");

                    var options = q.Options?.Select(o => new AssessmentOption
                    {
                        OptionText = o.OptionText,
                        OptionValue = o.OptionValue ?? 0
                    }).ToList() ?? new List<AssessmentOption>();

                    return new AssessmentQuestion
                    {
                        QuestionText = q.QuestionText,
                        QuestionType = q.QuestionType,
                        AssessmentOptions = options
                    };
                }).ToList();

                // Mapping assessment
                var assessment = new Assessment
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    AssessmentType = dto.AssessmentType,
                    AgeGroup = dto.AgeGroup,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    AssessmentQuestions = questions
                };

                // Lưu
                var id = await _service.CreateFullAssessmentAsync(assessment);

                return Ok(new
                {
                    message = "Assessment created successfully",
                    assessmentId = id
                });
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(new
                {
                    message = "Validation error",
                    error = argEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal Server Error",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
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
