using AutoMapper;
using DTO.Certificate;
using DTO.Course;
using DTO.CourseCategory;
using DTO.CourseQuestion;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DBContext;
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
		Task<bool> RegisterCourseAsync(int userId, int courseId);
		Task<CertificateDto> CompleteCourseAsync(int userId, int courseId);
	}

	public class CourseService : ICourseService
	{
		private readonly CourseRepository _repo;
		private readonly IMapper _mapper;
		private readonly CourseCategoryRepository _categoryRepo;
		private readonly Drug_use_prevention_systemContext _context;

		public CourseService(CourseRepository repo, IMapper mapper, CourseCategoryRepository categoryRepo, Drug_use_prevention_systemContext context)
		{
			_repo = repo;
			_mapper = mapper;
			_categoryRepo = categoryRepo;
			_context = context;
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
			var courses = await _repo.GetAllCourseAsync();
			return _mapper.Map<List<CourseDto>>(courses);
		}

		public async Task<Course?> GetCourseByIdAsync(int id)
		{
			return await _repo.GetByIdAsync(id);
		}

		public async Task<bool> RegisterCourseAsync(int userId, int courseId)
		{
			//check if user already registered for this course
			var hasRegistered = await _context.CourseRegisters.AnyAsync(cr => cr.UserId == userId && cr.CourseId == courseId);
			if (hasRegistered)
				throw new Exception("User is already registered for this course");
			
			//check if course exists
			var course = await _repo.GetByIdAsync(courseId);
			if (course == null)
			{
				throw new Exception("Course not found");
			}

			//check if user login
			var userExist = await _context.Users.AnyAsync(u => u.UserId == userId);
			if (!userExist)
				throw new Exception("User need to login to register");

			//check if user registered for this course successfully
			var registerExist = await _context.CourseRegisters.AnyAsync(cr => cr.UserId == userId && cr.CourseId == courseId);
			if (registerExist)
				throw new Exception("User is already registered for this course");

			var courseRegister = new CourseRegister
			{
				UserId = userId,
				CourseId = courseId,
				RegisterDate = DateTime.UtcNow,
			};

			_context.CourseRegisters.Add(courseRegister);
			await _context.SaveChangesAsync();
			return true;
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

		public async Task<CertificateDto> CompleteCourseAsync(int userId, int courseId)
		{
			// Validate user and course registration
			var isRegistered = _context.CourseRegisters.Any(cr => cr.UserId == userId && cr.CourseId == courseId);
			if (!isRegistered)
				throw new Exception("User is not registered for this course");

			// Check if course exists
			var course = await _repo.GetByIdAsync(courseId);
			if (course == null)
				throw new Exception("Course not found");

			// Check if user exists
			var user = await _context.Users.FindAsync(userId);
			if (user == null)
				throw new Exception("User not found");

			//Check if user has already completed the course
			var existingCertificate = await _context.Certifications
				.FirstOrDefaultAsync(c => c.UserId == userId && c.CourseId == courseId);
			if (existingCertificate != null)
				throw new Exception("User has already completed this course");

			var certificateCode = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();

			var certificate = new Certification
			{
				UserId = userId,
				CourseId = courseId,
				Name = $"{user.FullName} - {course.Title} Certificate",
				CertificationCode = certificateCode,
				CertificationUrl = $"https://example.com/certificates/{certificateCode}.pdf",
				AchievedDate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			_context.Certifications.Add(certificate);
			await _context.SaveChangesAsync();

			return _mapper.Map<CertificateDto>(certificate);

		}
	}
}
