using DTO.CourseQuestion.CourseOptions;
using Repositories.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Services
{

	public interface ICourseQuestionOptionService
	{
		Task<List<CourseQuestionOptionDto>> GetAllAsync();
		Task<CreateCourseOptionRequestDto?> CreateAsync(int questionId, CreateCourseOptionRequestDto optionDto);
		Task<UpdateCourseOptionRequestDto?> UpdateAsync(int id, UpdateCourseOptionRequestDto optionDto);
		Task<CourseQuestionOption?> DeleteAsync(int id);
	}
	public class CourseQuestionOptionService : ICourseQuestionOptionService
	{
		private readonly CourseQuestionOptionRepository _repo;
		private readonly IMapper _mapper;
		private readonly CourseQuestionRepository _questionRepo;

		public CourseQuestionOptionService(CourseQuestionOptionRepository repo, IMapper mapper, CourseQuestionRepository questionRepo)
		{
			_repo = repo;
			_mapper = mapper;
			_questionRepo = questionRepo;
		}

		public async Task<CreateCourseOptionRequestDto?> CreateAsync(int questionId, CreateCourseOptionRequestDto optionDto)
		{
			var question = await _questionRepo.GetByIdAsync(questionId);
			if (question == null)
				throw new Exception("Question not found");

			var courseOption = _mapper.Map<CourseQuestionOption>(optionDto);
			courseOption.QuestionId = questionId;

			var createdOption = await _repo.CreateAsync(courseOption);

			return _mapper.Map<CreateCourseOptionRequestDto>(createdOption);
		}

		public async Task<List<CourseQuestionOptionDto>> GetAllAsync()
		{
			var options = await _repo.GetAllAsync();
			return _mapper.Map<List<CourseQuestionOptionDto>>(options);
		}

		public async Task<UpdateCourseOptionRequestDto?> UpdateAsync(int id, UpdateCourseOptionRequestDto optionDto)
		{
			var optionModel = await _repo.GetByIdAsync(id);
			if (optionModel == null)
				return null;

			optionModel.OptionText = optionDto.OptionText;
			optionModel.OptionValue = optionDto.OptionValue;
			optionModel.DisplayOrder = optionDto.DisplayOrder;

			var updatedOption = _mapper.Map<CourseQuestionOption>(optionDto);
			await _repo.UpdateAsync(id, updatedOption);

			return _mapper.Map<UpdateCourseOptionRequestDto>(updatedOption);
		}

		public async Task<CourseQuestionOption?> DeleteAsync(int id)
		{
			var optionModel = await _repo.GetByIdAsync(id);
			if (optionModel == null)
				throw new Exception("No option found");

			await _repo.DeleteAsync(id);
			return optionModel;
		}

	}
}
