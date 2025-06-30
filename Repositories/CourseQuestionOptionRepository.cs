using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class CourseQuestionOptionRepository : GenericRepository<CourseQuestionOption>
	{
		public async Task<List<CourseQuestionOption>> GetAllAsync()
		{
			return await _context.CourseQuestionOptions.ToListAsync();
		}

		public async Task<CourseQuestionOption> CreateAsync(CourseQuestionOption options)
		{
			await _context.CourseQuestionOptions.AddAsync(options);
			await _context.SaveChangesAsync();
			return options;
		}

		public async Task<CourseQuestionOption?> UpdateAsync(int id, CourseQuestionOption options)
		{
			var optionModel = await _context.CourseQuestionOptions.FindAsync(id);
			if (optionModel == null)
				return null;
			optionModel.OptionText = options.OptionText;
			optionModel.OptionValue = options.OptionValue;
			optionModel.DisplayOrder = options.DisplayOrder;

		    _context.CourseQuestionOptions.Update(optionModel);
			await _context.SaveChangesAsync();

			return optionModel;
		}

		public async Task<CourseQuestionOption?> DeleteAsync(int id)
		{
			var option = await _context.CourseQuestionOptions.FindAsync(id);
			if (option == null)
				return null;
			_context.CourseQuestionOptions.Remove(option);
			await _context.SaveChangesAsync();
			return option;
		}

		public async Task<CourseQuestionOption?> GetByIdAsync(int id)
		{
			return await _context.CourseQuestionOptions.FindAsync(id);
		}
	}
}
