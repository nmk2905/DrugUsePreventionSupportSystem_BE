using API.Dtos.Course;
using API.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.DBContext;
using Repositories.Models;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseController : ControllerBase
	{
		private readonly ICourseRepository _repo;
		public CourseController(ICourseRepository repo)
		{
			_repo = repo;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var courseModel = await _repo.GetAllAsync();
			var courseDto = courseModel.Select(c => c.ToCourseDto());
			return Ok(courseDto);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			var course = await _repo.GetByIdAsync(id);

			if (course == null) 
				return NotFound();

			return Ok(course);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCourse([FromBody] CreateStockRequest courseRequest)
		{
			var courseDto = courseRequest.ToCourseFromCreateDto();
			await _repo.CreateAsync(courseDto);
			return CreatedAtAction(nameof(GetById), new {id = courseDto.CourseId}, courseDto.ToCourseDto());
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCourse([FromRoute] int id, [FromBody] UpdateCourseDto update)
		{
			var courseModel = await _repo.UpdateAsync(id, update.ToCourseFromUpdateDto());

			if (courseModel == null)
				return NotFound();

			return Ok(courseModel);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id) 
		{
			var courseModel = await _repo.DeleteAsync(id);
			
			if (courseModel == null)
				return NotFound();

			return NoContent();
		}
	}
}
