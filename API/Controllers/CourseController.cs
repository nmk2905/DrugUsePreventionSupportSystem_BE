//using API.Dtos.Course;
//using API.Mappers;
using DTO.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Repositories;
using Repositories.DBContext;
using Repositories.Models;
using Services;

namespace api.controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CourseController : ControllerBase
	{
		private readonly ICourseService _service;

		public CourseController(ICourseService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var courseModel = await _service.GetAllCoursesAsync();
			return Ok(courseModel);
		}

		//		[HttpGet("{id}")]
		//		public async Task<IActionResult> GetById([FromRoute] int id)
		//		{
		//			var course = await _service.GetCourseByIdAsync(id);

		//			if (course == null)
		//				return NotFound();

		//			return Ok(course);
		//		}

		[HttpPost]
		public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequestDto courseRequest)
		{
			var courseModel = await _service.CreateCourseAsync(courseRequest);
			return Ok(courseModel);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCourse([FromRoute] int id, [FromBody] UpdateCourseRequestDto course)
		{
			var courseModel = await _service.UpdateCourseAsync(id, course);

			if (courseModel == null)
				return NotFound();

			return Ok(courseModel);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			var courseModel = await _service.DeleteCourseAsync(id);

			if (courseModel == null)
				return NotFound();

			return NoContent();
		}
	}
}
