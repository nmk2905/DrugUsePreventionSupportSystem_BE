using AutoMapper;
using DTO.CourseCategory;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public interface ICourseCategory
	{
		Task<List<CourseCategory>> GetAllCategorySync();
		Task<CreateCourseCategoryRequestDto?> CreateCategoryAsync(CreateCourseCategoryRequestDto category);
		Task<CourseCategoryDto?> UpdateCategoryAsync(int id, UpdateCourseCategoryRequestDto categoryDto);
	}
	public class CourseCategoryService : ICourseCategory
	{
		private readonly CourseCategoryRepository _repo;
		private readonly IMapper _mapper;

		public CourseCategoryService(CourseCategoryRepository repo, IMapper mapper) { 
			_repo = repo;
			_mapper = mapper;
		}

		public async Task<CreateCourseCategoryRequestDto?> CreateCategoryAsync(CreateCourseCategoryRequestDto categoryDto)
		{
			var categoryModel = _mapper.Map<CourseCategory>(categoryDto);
			await _repo.CreateAsync(categoryModel);
			return _mapper.Map<CreateCourseCategoryRequestDto>(categoryDto);
		}

		public async Task<List<CourseCategory>> GetAllCategorySync()
		{
			return await _repo.GetAllAsync();
		}

		public async Task<CourseCategoryDto?> UpdateCategoryAsync(int id, UpdateCourseCategoryRequestDto categoryDto)
		{
			var categoryModel = await _repo.GetByIdAsync(id);

			if (categoryModel == null) 
				return null;

			categoryModel.Name = categoryDto.Name;
			categoryModel.Description = categoryDto.Description;
			categoryModel.Age = categoryDto.Age;

			await _repo.UpdateAsync(categoryModel);
			return _mapper.Map<CourseCategoryDto>(categoryModel);
		}
	}
}
