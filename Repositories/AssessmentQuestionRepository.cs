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
    public class AssessmentQuestionRepository : GenericRepository<AssessmentQuestion>
    {
        public AssessmentQuestionRepository() { }

        public async Task<List<AssessmentQuestion>> GetAllAssessmentQuestionAsync()
        {
            return _context.AssessmentQuestions.Include(b => b.AssessmentId).ToList();
        }
        public async Task<AssessmentQuestion> GetAssessmentQuestionById(int id)
        {
            return await _context.AssessmentQuestions.Include(b => b.AssessmentId).FirstOrDefaultAsync(i => i.QuestionId == id);
        }
    }
}
