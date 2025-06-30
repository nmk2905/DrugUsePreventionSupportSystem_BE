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
    public class AssessmentOptionRepository : GenericRepository<AssessmentOption>
    {
        public AssessmentOptionRepository() { }

        public async Task<List<AssessmentOption>> GetAllAssessmentOption()
        {
            return await _context.AssessmentOptions.Include(b => b.Question).ToListAsync();
        }
        public async Task<AssessmentOption> GetAssessmentOptionById(int id)
        {
            return await _context.AssessmentOptions.Include(b => b.Question).FirstOrDefaultAsync(i => i.OptionId == id);
        }
    }
}
