using DTO.AssessmentType;
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
    public class AssessmentTypeController : Controller
    {
        private readonly IAssessmentTypeService _service;
        
        public AssessmentTypeController(IAssessmentTypeService service)
        {
            _service = service;
        }

       
        [HttpGet]
        public async Task<ActionResult<List<AssessmentType>>> GetAll()
        {
            var data = await _service.GetAllAssessmentType();
            var result = data.Select(dto => new AssessmentTypeSampleDto
            {
                AssessmentTypeId = dto.AssessmentTypeId,
                Name = dto.Name,
                Description = dto.Description
            }).ToList();
            return Ok(result);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentType>> GetById(int id)
        {
            var dto = await _service.GetAssessmentTypeById(id);
            if (dto == null)
                return NotFound(new { message = "Không tìm thấy." });

            var result = new AssessmentTypeSampleDto
            {
                AssessmentTypeId = dto.AssessmentTypeId,
                Name = dto.Name,
                Description = dto.Description
            };
            return Ok(result);
        }


     
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AssessmentTypeDto dto)
        {
            var type = new AssessmentType
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var id = await _service.AddAssessmentTypeAsync(type);
            return Ok(new { message = "Thêm thành công"});
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] AssessmentTypeDto dto)
        {
            var existing = await _service.GetAssessmentTypeById(id);
            if (existing == null)
                return NotFound(new { message = "Không tìm thấy." });

            if (dto.Name != null)
                existing.Name = dto.Name;

            if (dto.Description != null)
                existing.Description = dto.Description;

            await _service.UpdateAssessmentTypeAsync(existing);
            return Ok(new { message = "Cập nhật thành công" });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAssessmentTypeAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Xóa thành công" });
        }


        
    }
}
