using AutoMapper;
using DTO.Course;
using DTO.CourseCategory;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services
{
	public interface ICourseService
	{
		Task<List<CourseDto>> GetAllCoursesAsync();
		Task<CourseDto?> CreateCourseAsync(CreateCourseRequestDto course);
		Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseRequestDto course);
		Task<Course?> DeleteCourseAsync(int id);
		Task<Course?> GetCourseByIdAsync(int id);
	}

	public class CourseService : ICourseService
	{
		private readonly CourseRepository _repo;
		private readonly IMapper _mapper;
		private readonly CourseCategoryRepository _categoryRepo;

		public CourseService(CourseRepository repo, IMapper mapper, CourseCategoryRepository categoryRepo)
		{
			_repo = repo;
			_mapper = mapper;
			_categoryRepo = categoryRepo;
		}

		public async Task<CourseDto?> CreateCourseAsync(CreateCourseRequestDto courseDto)
		{
			if (courseDto.Category.HasValue)
			{
				var categoryExists = await _categoryRepo.ExistsAsync(courseDto.Category.Value);
				if (!categoryExists)
					throw new Exception("Invalid category ID");
			}

			var course = _mapper.Map<Course>(courseDto);
			await _repo.CreateAsync(course);
			var savedCourse = await _repo.GetByIdAsync(course.CourseId);
			return _mapper.Map<CourseDto>(savedCourse);
		}

		public async Task<Course?> DeleteCourseAsync(int id)
		{
			return await _repo.DeleteAsync(id);
		}

		public async Task<List<CourseDto>> GetAllCoursesAsync()
		{
			var course = await _repo.GetAllCourseAsync();
			return course.Select(MapToDto).ToList();
		}

		public async Task<Course?> GetCourseByIdAsync(int id)
		{
			return await _repo.GetByIdAsync(id);
		}

		public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseRequestDto courseDto)
		{
			var course = await _repo.GetByIdAsync(id);

			if (course == null) 
				return null;

			if (courseDto.CategoryId.HasValue)
			{
				var categoryExists = await _repo.ExistsAsync(courseDto.CategoryId.Value);
				if (!categoryExists)
					throw new Exception("Invalid category ID");
			}

			course.Title = courseDto.Title;
			course.Description = courseDto.Description;
			course.Duration = courseDto.Duration;
			course.VideoUrl = courseDto.VideoUrl;
			course.DocumentContent = courseDto.DocumentContent;
			course.Category = courseDto.CategoryId;

		    await _repo.UpdateAsync(id, course);
			var savedCourse = await _repo.GetByIdAsync(id);
			return _mapper.Map<CourseDto>(savedCourse);
		}

		private CourseDto MapToDto(Course course)
		{
			return new CourseDto
			{
				CourseId = course.CourseId,
				Title = course.Title,
				Description = course.Description,
				Duration = course.Duration,
				CreatedAt = course.CreatedAt,
				VideoUrl = course.VideoUrl,
				DocumentContent = course.DocumentContent,
				Category = course.CategoryNavigation != null ? new CourseCategoryDto
				{
					CategoryId = course.CategoryNavigation.CategoryId,
					Name = course.CategoryNavigation.Name,
					Description = course.CategoryNavigation.Description,
					Age = course.CategoryNavigation.Age
				} : null
			};
		}
	}
}
