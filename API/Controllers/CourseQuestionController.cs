using DTO.CourseQuestion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseQuestionController : ControllerBase
	{
		private readonly ICourseQuestionService _service;

		public CourseQuestionController(ICourseQuestionService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var questions = await _service.GetAllAsync();
			return Ok(questions);
		}

		[HttpPost("{id}")]
		public async Task<IActionResult> Create([FromRoute] int id, [FromBody] CreateQuestionRequestDto questionDto)
		{
			try
			{
				var createdQuestion = await _service.CreateAsync(id, questionDto);
				return Ok(createdQuestion);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody]UpdateCourseQuestionRequestDto questionDto)
		{
			var updatedQuestion = await _service.UpdateAsync(id, questionDto);
			if (updatedQuestion == null)
			{
				return NotFound();
			}
			return Ok(updatedQuestion);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var deletedQuestion = await _service.DeleteAsync(id);
			if (deletedQuestion == null)
			{
				return NotFound();
			}
			return NoContent();
		}
	}
}
