//using API.Dtos.Course;
//using API.Mappers;
using DTO.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Repositories;
using Repositories.DBContext;
using Repositories.Models;
using Services;
using System.Security.Claims;

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

		[HttpPost("Register/{courseId}")]
		[Authorize(Roles = "3")]
		public async Task<IActionResult> RegisterCourse([FromRoute] int courseId)
		{
			try
			{
				var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

				var sucess = await _service.RegisterCourseAsync(userId, courseId);
				if (!sucess)
					return BadRequest(new { message = "Failed to register" });

				return Ok(new { message = "Course register successfully" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
