using DTO.CourseQuestion.CourseOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Services;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseOptionController : ControllerBase
	{
		private readonly ICourseQuestionOptionService _service;

		public CourseOptionController(ICourseQuestionOptionService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllOptions()
		{
			var options = await _service.GetAllAsync();
			return Ok(options);
		}

		[HttpPost("{questionId}")]
		public async Task<IActionResult> CreateOption([FromRoute] int questionId, CreateCourseOptionRequestDto questionDto)
		{
			try
			{
				var question = await _service.CreateAsync(questionId, questionDto);
				return Ok(question);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}

		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOption([FromRoute] int id, UpdateCourseOptionRequestDto optionDto)
		{
			try
			{
				var option = await _service.UpdateAsync(id, optionDto);
				return Ok(option);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOption(int id)
		{
			try
			{
				var option = await _service.DeleteAsync(id);
				return Content("Delete successfully!");
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}

		}
	}
}
