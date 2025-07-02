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
    public class AssessmentOptionController :Controller
    {
        private readonly IAssessmentOptionService _service;

        public AssessmentOptionController(IAssessmentOptionService assessmentOptionService)
        {
            _service = assessmentOptionService;
        }

        //lấy toàn bộ đáp án 
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAssessmentOption();
            var result = data.Select(q => new OptionSampleDto
            {
                OptionId = q.OptionId,
                QuestionId = q.QuestionId,
                QuestionText = q.Question?.QuestionText,
                OptionText = q.OptionText,
                OptionValue = q.OptionValue
            }).ToList();

            return Ok(result);
        }

        //lấy data 1 đáp án chỉ định
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var q = await _service.GetAssessmentOptionById(id);
            if (q == null) return NotFound();

            var result = new OptionSampleDto
            {
                OptionId = q.OptionId,
                QuestionId = q.QuestionId,
                QuestionText = q.Question?.QuestionText,
                OptionText = q.OptionText,
                OptionValue = q.OptionValue
            };

            return Ok(result);
        }

        //thêm 1 câu trả lời
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddOptionDto dto)
        {
            var option = new AssessmentOption
            {
                QuestionId = dto.QuestionId,
                OptionText = dto.OptionText,
                OptionValue = dto.OptionValue
            };

            var id = await _service.AddOptionAsync(option);
            return Ok(new { message = "Thêm thành công", optionId = id });
        }

        //update 1 câu trả lời
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOptionDto dto)
        {
            var existing = await _service.GetAssessmentOptionById(id);
            if (existing == null)
                return NotFound(new { message = "Không tìm thấy câu trả lời." });

            if (dto.QuestionId != null)
                existing.QuestionId = dto.QuestionId;

            if (!string.IsNullOrWhiteSpace(dto.OptionText))
                existing.OptionText = dto.OptionText;

            if (dto.OptionValue !=null)
                existing.OptionValue = dto.OptionValue;

            await _service.UpdateOptionAsync(existing);
            return Ok(new { message = "Cập nhật thành công" });
        }

        //xóa 1 câu trả lời
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteOptionAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Xóa thành công" });
        }


        //get
        public class OptionSampleDto
        {
            public int OptionId { get; set; }
            public int? QuestionId { get; set; }
            public string QuestionText { get; set; }
            public string OptionText { get; set; }
            public int? OptionValue { get; set; }
        }

        //add,update
        public class AddOptionDto
        {
            public int? QuestionId { get; set; }
            public string OptionText { get; set; }
            public int? OptionValue { get; set; }
        }

        public class UpdateOptionDto
        {
            public int? QuestionId { get; set; }
            public string OptionText { get; set; }
            public int? OptionValue { get; set; }
        }
    }
}
