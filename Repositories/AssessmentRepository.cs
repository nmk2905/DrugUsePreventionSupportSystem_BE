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
    public class AssessmentRepository : GenericRepository<Assessment>
    {
        public AssessmentRepository() { }

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
                .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }
    }
}
