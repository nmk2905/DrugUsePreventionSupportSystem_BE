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
    public class AgeGroupController : Controller
    {
        private readonly IAgeGroupService _service;

        public AgeGroupController(IAgeGroupService ageGroupService)
        {
            _service = ageGroupService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AgeGroup>>> GetAll()
        {
            var data = await _service.GetAllAgeGroup();
            var result = data.Select(dto => new AgeGroupSampleDto
            {
                GroupId = dto.GroupId,
                Name = dto.Name,
                Description = dto.Description,
                MinAge = dto.MinAge,
                MaxAge = dto.MaxAge
            }).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgeGroup>> GetById(int id)
        {
            var dto = await _service.GetAgeGroupById(id);
            if (dto == null)
                return NotFound(new { message = "Không tìm thấy nhóm tuổi." });

            var result = new AgeGroupSampleDto
            {
                GroupId = dto.GroupId,
                Name = dto.Name,
                Description = dto.Description,
                MinAge = dto.MinAge,
                MaxAge = dto.MaxAge
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AgeGroupDto dto)
        {
            if (dto.MinAge == null || dto.MaxAge == null)
                return BadRequest(new { message = "MinAge và MaxAge là bắt buộc." });

            if (dto.MinAge > dto.MaxAge)
                return BadRequest(new { message = "MinAge không thể lớn hơn MaxAge." });

            var age = new AgeGroup
            {
                Name = dto.Name,
                Description = dto.Description,
                MinAge = dto.MinAge.Value,
                MaxAge = dto.MaxAge.Value
            };

            var id = await _service.AddAgeGroupAsync(age);
            return Ok(new { message = "Thêm thành công", groupId = id });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] AgeGroupDto age)
        {
            var existing = await _service.GetAgeGroupById(id);
            if (existing == null)
                return NotFound(new { message = "Không tìm thấy nhóm tuổi." });

            if (age.Name != null)
                existing.Name = age.Name;

            if (age.Description != null)
                existing.Description = age.Description;

            if (age.MinAge.HasValue)
                existing.MinAge = age.MinAge.Value;

            if (age.MaxAge.HasValue)
                existing.MaxAge = age.MaxAge.Value;

            if (age.MinAge.HasValue && age.MaxAge.HasValue && age.MinAge > age.MaxAge)
                return BadRequest(new { message = "MinAge không thể lớn hơn MaxAge." });

            await _service.UpdateAgeGroupAsync(existing);
            return Ok(new { message = "Cập nhật thành công" });
        }



        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAgeGroupAsync(id);
            if (!success) return NotFound();

            return Ok(new { message = "Xóa thành công" });
        }

        //get
        public class AgeGroupSampleDto
        {
            public int GroupId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int MinAge { get; set; }
            public int MaxAge { get; set; }
        }

        //add,update
        public class AgeGroupDto
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public int? MinAge { get; set; }
            public int? MaxAge { get; set; }
        }
    }
}
