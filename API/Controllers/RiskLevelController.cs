using DTO.RiskLevel;
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
    public class RiskLevelController : Controller
    {
        private readonly IRiskLevelService _service;
        public RiskLevelController(IRiskLevelService service)
        {
            _service = service;
        }

        //lấy toàn bộ RiskLevel
        [HttpGet]
        [ProducesResponseType(typeof(List<RiskLevelSampleDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RiskLevel>>> GetAll()
        {
            var data = await _service.GetAllRiskLevel();
            var result = data.Select(dto => new RiskLevelSampleDto
            {
                RiskId = dto.RiskId,
                RiskLevel1 = dto.RiskLevel1,
                RiskDescription = dto.RiskDescription
            }).ToList();
            return Ok(result);
        }

        //lấy 1 RiskLevel theo id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<RiskLevelSampleDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<RiskLevel>> GetById(int id)
        {
            var dto = await _service.GetRiskLevelById(id);
            if (dto == null)
                return NotFound(new { message = "Không tìm thấy." });

            var result = new RiskLevelSampleDto
            {
                RiskId = dto.RiskId,
                RiskLevel1 = dto.RiskLevel1,
                RiskDescription = dto.RiskDescription
            };
            return Ok(result);
        }

        //tạo RiskLevel
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RiskLevelDto dto)
        {
            var age = new RiskLevel
            {
                RiskLevel1 = dto.RiskLevel1,
                RiskDescription = dto.RiskDescription
            };

            var id = await _service.AddRiskLevelAsync(age);
            return Ok(new { message = "Thêm thành công", riskId = id });
        }

        //update RiskLevel
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RiskLevelDto dto)
        {
            var existing = await _service.GetRiskLevelById(id);
            if (existing == null)
                return NotFound(new { message = "Không tìm thấy." });

            if (dto.RiskLevel1 != null)
                existing.RiskLevel1 = dto.RiskLevel1;

            if (dto.RiskDescription != null)
                existing.RiskDescription = dto.RiskDescription;

            await _service.UpdateRiskLevelAsync(existing);
            return Ok(new { message = "Cập nhật thành công" });
        }

        //xóa
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteRiskLevelAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Xóa thành công" });
        }


    }
}
