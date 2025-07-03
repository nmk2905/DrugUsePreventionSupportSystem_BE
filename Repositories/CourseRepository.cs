using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;

namespace Repositories
{

	public class CourseRepository : GenericRepository<Course>
	{
		public CourseRepository() { }

		public async Task<Course?> CreateAsync(Course course)
		{
			await _context.Courses.AddAsync(course);
			await _context.SaveChangesAsync();
			return await _context.Courses.Include(c => c.CategoryNavigation)
										 .FirstOrDefaultAsync(c => c.CourseId == course.CourseId);
		}

		public async Task<Course?> DeleteAsync(int id)
		{
			var courseModel = await _context.Courses.Include(c => c.CourseQuestions)
													.FirstOrDefaultAsync(c => c.CourseId == id);

			if (courseModel == null)
				return null;

			_context.Courses.Remove(courseModel);
			await _context.SaveChangesAsync();

			return courseModel;
		}

		public async Task<List<Course>> GetAllCourseAsync()
		{
			return await _context.Courses.Include(c => c.CategoryNavigation)
										 .Include(c => c.CourseQuestions)
										 .ThenInclude(cq => cq.CourseQuestionOptions)
										 .ToListAsync();
		}

		public async Task<Course?> GetByIdAsync(int id)
		{
			return await _context.Courses.Include(c => c.CategoryNavigation)
										 .Include(c => c.CourseQuestions)
										 .FirstOrDefaultAsync(i => i.CourseId == id);
		}

		public async Task<Course?> UpdateAsync(int id, Course course)
		{
			var courseModel = await _context.Courses.Include(c => c.CategoryNavigation)
													.FirstOrDefaultAsync(c => c.CourseId == id);

			if (courseModel == null)
				return null;

			courseModel.Title = course.Title;
			courseModel.Description = course.Description;
			courseModel.Duration = course.Duration;
			courseModel.VideoUrl = course.VideoUrl;
			courseModel.DocumentContent = course.DocumentContent;

			await _context.SaveChangesAsync();

			return courseModel;
		}


	}
}
