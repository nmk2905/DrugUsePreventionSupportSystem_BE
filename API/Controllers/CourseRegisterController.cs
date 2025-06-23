using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseRegisterController : ControllerBase
	{
		public ICourseRegisterService _service;

		public CourseRegisterController(ICourseRegisterService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var courseRegisters = await _service.GetAllAsync();
			return Ok(courseRegisters);
		}
	}
}
