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
    public class AssessmentTypeRepository : GenericRepository<AssessmentType>
    {
        public AssessmentTypeRepository() { }

        public async Task<List<AssessmentType>> GetAllAssessmentType()
        {
            return await _context.AssessmentTypes.ToListAsync();
        }
        public async Task<AssessmentType> GetAssessmentTypeById(int id)
        {
            return await _context.AssessmentTypes.FirstOrDefaultAsync(i => i.AssessmentTypeId == id);
        }
    }
}
