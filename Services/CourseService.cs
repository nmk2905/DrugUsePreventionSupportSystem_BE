using AutoMapper;
using DTO.Course;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public CourseService(CourseRepository repo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<CourseDto?> CreateCourseAsync(CreateCourseRequestDto courseDto)
		{
			var course = _mapper.Map<Course>(courseDto);
			await _repo.CreateAsync(course);
			return _mapper.Map<CourseDto>(course);
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

			course.Title = courseDto.Title;
			course.Description = courseDto.Description;
			course.Duration = courseDto.Duration;
			course.VideoUrl = courseDto.VideoUrl;
			course.DocumentContent = courseDto.DocumentContent;

			var update = await _repo.UpdateAsync(id, course);
			return _mapper.Map<CourseDto>(update);
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
				DocumentContent = course.DocumentContent
			};
		}
	}
}
