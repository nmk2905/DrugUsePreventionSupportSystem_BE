using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.DBContext;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AssessmentRepository : GenericRepository<Assessment>
    {
        public AssessmentRepository(Drug_use_prevention_systemContext context) : base(context)
        {
        }

        public async Task<List<Assessment>> GetAllAssessment()
        {
            return await _context.Assessments
                .Include(a => a.AgeGroupNavigation)           
                .Include(a => a.AssessmentTypeNavigation)     
                .ToListAsync();
        }
        public async Task<Assessment> GetAssessmentById(int id)
        {
            return await _context.Assessments
                .Include(a => a.AgeGroupNavigation)
                .Include(a => a.AssessmentTypeNavigation)
                .Include(a => a.AssessmentQuestions)
                    .ThenInclude(q => q.AssessmentOptions)
                .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        //
        public async Task<int> AddAssessmentAsync(Assessment assessment)
        {
            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();
            return assessment.AssessmentId;
        }
    }
}
