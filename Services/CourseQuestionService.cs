using AutoMapper;
using DTO.CourseQuestion;
using Repositories;
using Repositories.DBContext;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public interface ICourseQuestionService
	{
		Task<List<CourseQuestionDto>> GetAllAsync();
		Task<CreateQuestionRequestDto> CreateAsync(int courseId, CreateQuestionRequestDto question);
		Task<CourseQuestion?> UpdateAsync(int id, UpdateCourseQuestionRequestDto question);
		Task<CourseQuestion?> DeleteAsync(int id);
	}
	public class CourseQuestionService : ICourseQuestionService
	{
		private readonly CourseQuestionRepository _repo;
		private readonly IMapper _mapper;
		private readonly CourseRepository _courseRepo;

		public CourseQuestionService(CourseQuestionRepository repo, IMapper mapper, CourseRepository courseRepo, Drug_use_prevention_systemContext context)
		{
			_repo = repo;
			_mapper = mapper;
			_courseRepo = courseRepo;
		}

		public async Task<CreateQuestionRequestDto> CreateAsync(int courseId, CreateQuestionRequestDto questionDto)
		{
			var course = await _courseRepo.GetByIdAsync(courseId);
			if (course == null)
				throw new Exception("Course not found");

			var courseQuestion = _mapper.Map<CourseQuestion>(questionDto);
			courseQuestion.CourseId = courseId;

			var createdQuestion = await _repo.CreateAsync(courseQuestion);

			return _mapper.Map<CreateQuestionRequestDto>(createdQuestion);
		}

		public async Task<CourseQuestion?> DeleteAsync(int id)
		{
			return await _repo.DeleteAsync(id);
		}

		public async Task<List<CourseQuestionDto>> GetAllAsync()
		{
			var question = await _repo.GetAllAsync();
			return _mapper.Map<List<CourseQuestionDto>>(question);
		}

		public async Task<CourseQuestion?> UpdateAsync(int id, UpdateCourseQuestionRequestDto questionDto)
		{
			var updatedQuestion = await _repo.GetByIdAsync(id);

			if (updatedQuestion == null)
			{
				throw new Exception("Question not found");
			}

			updatedQuestion.QuestionText = questionDto.QuestionText;
			updatedQuestion.DisplayOrder = questionDto.DisplayOrder;

			await _repo.UpdateAsync(id, updatedQuestion);
			return updatedQuestion;
		}
	}
}
