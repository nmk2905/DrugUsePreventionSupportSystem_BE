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
	public class CourseQuestionRepository : GenericRepository<CourseQuestion>
	{
		public async Task<List<CourseQuestion>> GetAllAsync()
		{
			return await _context.CourseQuestions.Include(q => q.CourseQuestionOptions)
												 .ToListAsync();
		}

		public async Task<CourseQuestion> CreateAsync(CourseQuestion question)
		{
			_context.CourseQuestions.Add(question);
			await _context.SaveChangesAsync();
			return question;
		}

		public async Task<CourseQuestion?> DeleteAsync(int id)
		{
			var question = await _context.CourseQuestions.FindAsync(id);
			if (question == null)
				return null;
			_context.CourseQuestions.Remove(question);
			await _context.SaveChangesAsync();
			return question;
		}

		public async Task<CourseQuestion?> UpdateAsync(int id, CourseQuestion question)
		{
			var questionModel = await _context.CourseQuestions.FindAsync(id);

			if (questionModel == null)
				return null;

			questionModel.QuestionText = question.QuestionText;
			questionModel.DisplayOrder = question.DisplayOrder;

			_context.CourseQuestions.Update(questionModel);
			await _context.SaveChangesAsync();

			return questionModel;
		}

		public async Task<CourseQuestion?> GetByIdAsync(int id)
		{
			return await _context.CourseQuestions.FindAsync(id);
		}
	}
}
