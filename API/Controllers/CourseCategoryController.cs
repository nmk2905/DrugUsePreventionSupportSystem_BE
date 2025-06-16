using DTO.CourseCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseCategoryController : ControllerBase
	{
		private readonly ICourseCategory _service;
		public CourseCategoryController(ICourseCategory service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCourseCategory()
		{
			var courseModel = await _service.GetAllCategorySync();
			return Ok(courseModel);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCourseCategory(CreateCourseCategoryRequestDto courseDto)
		{
			var countModel = await _service.CreateCategoryAsync(courseDto);
			return Ok(countModel);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCourseCategory([FromRoute] int id, UpdateCourseCategoryRequestDto categoryDto)
		{
			var categoryModel = await _service.UpdateCategoryAsync(id, categoryDto);

			if (categoryModel != null) 
				return NoContent();

			return Ok(categoryModel);
		}
	}
}
