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
	public class UserAnswerRepository : GenericRepository<UserAnswer>
	{
		public UserAnswerRepository() { }

		public async Task<List<UserAnswer>> GetAllAsync()
		{
			return await _context.UserAnswers.ToListAsync();
		}

		public async Task<UserAnswer?> CreateAsync(UserAnswer userAnswer)
		{
			await _context.UserAnswers.AddAsync(userAnswer);
			await _context.SaveChangesAsync();
			return userAnswer;
		}

		public async Task<bool> HasAnsweredAsync(int userId, int questionId, int courseId)
		{
			return await _context.UserAnswers
				.AnyAsync(ua => ua.UserId == userId && ua.QuestionId == questionId && ua.CourseId == courseId);
		}
	}
}
