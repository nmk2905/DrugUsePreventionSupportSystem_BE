using AutoMapper;
using DTO.UserAnswer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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

	public interface IUserAnswerService
	{
		Task<List<UserAnswerDto>> GetAllAsync();
		Task<UserAnswerDto> SubmitAnswerAsync(int userId, int courseId, SubmitAnswerRequestDto answerDto);
	}

	public class UserAnswerService : IUserAnswerService
	{
		private readonly UserAnswerRepository _repo;
		private readonly CourseRepository _courseRepo;
		private readonly CourseQuestionRepository _questionRepo;
		private readonly IMapper _mapper;
		private readonly Drug_use_prevention_systemContext _context;

		public UserAnswerService(UserAnswerRepository repo, CourseRepository courseRepo, CourseQuestionRepository questionRepo, IMapper mapper, Drug_use_prevention_systemContext context)
		{
			_repo = repo;
			_courseRepo = courseRepo;
			_questionRepo = questionRepo;
			_mapper = mapper;
			_context = context;
		}

		public async Task<List<UserAnswerDto>> GetAllAsync()
		{
			var aswer = await _repo.GetAllAsync();
			return _mapper.Map<List<UserAnswerDto>>(aswer);
		}

		public async Task<UserAnswerDto> SubmitAnswerAsync(int userId, int courseId, SubmitAnswerRequestDto answerDto)
		{
			//Check if the user has registered for the course
			var registation = await _context.CourseRegisters.FirstOrDefaultAsync(cr => cr.UserId == userId && cr.CourseId == courseId);

			if (registation == null)
				throw new Exception("User is not registered for this course.");

			//Check if the question or option exists
			var question = await _questionRepo.GetQuestionsByCourseIdAsync(courseId);
			var selectedOption = question
					.Where(q => q.QuestionId == answerDto.QuestionId)
					.SelectMany(q => q.CourseQuestionOptions)
					.FirstOrDefault(o => o.OptionId == answerDto.OptionId);

			if (selectedOption == null)
				throw new Exception("Question or option does not exist.");

			//Check if the user has already answered this question
			var hasAnswered = await _repo.HasAnsweredAsync(userId, answerDto.QuestionId, courseId);
			if (hasAnswered)
				throw new Exception("User has already answered this question.");

			//Calculate total points
			var existingAnswer = await _context.UserAnswers
					.Where(ua => ua.UserId == userId && ua.CourseId == courseId)
					.Include(ua => ua.Option)
					.ToListAsync();
			int totalPoint = existingAnswer.Sum(ua => ua.Option?.OptionValue ?? 0) + (selectedOption.OptionValue ?? 0);

			// Validate totalPoint
			if (totalPoint < 0) // Trường hợp bất thường (nên không xảy ra với logic hiện tại)
				throw new Exception("Invalid total points calculation.");

			var userAnswer = new UserAnswer
			{
				UserId = userId,
				CourseId = courseId,
				QuestionId = answerDto.QuestionId,
				OptionId = answerDto.OptionId,
				AnswerAt = DateTime.UtcNow,
				TotalPoint = totalPoint
			};

			var createdAnswer = await _repo.CreateAsync(userAnswer);
			return _mapper.Map<UserAnswerDto>(createdAnswer);

		}
	}
}
