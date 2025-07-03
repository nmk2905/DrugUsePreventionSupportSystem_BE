using DTO.UserAnswer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserAnswerController : ControllerBase
	{
		private readonly IUserAnswerService _service;

		public UserAnswerController(IUserAnswerService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var answers = await _service.GetAllAsync();
			return Ok(answers);
		}

		[HttpPost("{courseId}")]
		public async Task<IActionResult> SubmitAnswer([FromRoute] int courseId, SubmitAnswerRequestDto answerDto)
		{
			try
			{
				var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
				var answer = await _service.SubmitAnswerAsync(userId, courseId, answerDto);
				return Ok(answer);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
