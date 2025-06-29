using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class AssessmentQuestionController : Controller
    {
        private readonly IAssessmentQuestionService _service;

        public AssessmentQuestionController(IAssessmentQuestionService AssessmentQuestionService)
        {
            _service = AssessmentQuestionService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAssessmentQuestion();
            var result = data.Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                AssessmentTitle = q.Assessment?.Title
            }).ToList();

            return Ok(result);
        }

        // GET: api/AssessmentQuestion/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var q = await _service.GetAssessmentQuestionById(id);
            if (q == null) return NotFound();

            var result = new QuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                AssessmentTitle = q.Assessment?.Title
            };

            return Ok(result);
        }

        // POST: api/AssessmentQuestion/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CreateQuestionDto dto)
        {
            var validTypes = new[] { "single choice", "multiple choice" };
            if (string.IsNullOrWhiteSpace(dto.QuestionType) ||
                !validTypes.Contains(dto.QuestionType.Trim().ToLower()))
            {
                return BadRequest(new { message = "QuestionType chỉ được là 'single choice' hoặc 'multiple choice'" });
            }

            var question = new AssessmentQuestion
            {
                AssessmentId = dto.AssessmentId,
                QuestionText = dto.QuestionText,
                QuestionType = dto.QuestionType.Trim().ToLower()
            };

            var id = await _service.AddQuestionAsync(question);
            return Ok(new { message = "Thêm thành công", questionId = id });
        }


        // PUT: api/AssessmentQuestion/Update/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateQuestionDto dto)
        {
            var existing = await _service.GetAssessmentQuestionById(id);
            if (existing == null)
                return NotFound(new { message = "Không tìm thấy câu hỏi." });

            var validTypes = new[] { "single choice", "multiple choice" };
            if (!string.IsNullOrWhiteSpace(dto.QuestionType) &&
                !validTypes.Contains(dto.QuestionType.Trim().ToLower()))
            {
                return BadRequest(new { message = "QuestionType chỉ được là 'single choice' hoặc 'multiple choice'" });
            }

            if (dto.AssessmentId != null)
                existing.AssessmentId = dto.AssessmentId;

            if (!string.IsNullOrWhiteSpace(dto.QuestionText))
                existing.QuestionText = dto.QuestionText;

            if (!string.IsNullOrWhiteSpace(dto.QuestionType))
                existing.QuestionType = dto.QuestionType.Trim().ToLower();

            await _service.UpdateQuestionAsync(existing);
            return Ok(new { message = "Cập nhật thành công" });
        }



        // DELETE: api/AssessmentQuestion/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteQuestionAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Xóa thành công" });
        }

        // DTO cho Get
        public class QuestionDto
        {
            public int QuestionId { get; set; }
            public string QuestionText { get; set; }
            public string QuestionType { get; set; }
            public string AssessmentTitle { get; set; }
        }

        // DTO cho Add/Update
        public class CreateQuestionDto
        {
            public int? AssessmentId { get; set; }
            public string QuestionText { get; set; }
            public string QuestionType { get; set; }
        }

        public class UpdateQuestionDto
        {
            public int? AssessmentId { get; set; }
            public string? QuestionText { get; set; }
            public string? QuestionType { get; set; }
        }
    }
}
