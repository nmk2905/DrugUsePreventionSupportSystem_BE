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
    public class AgeGroupRepository : GenericRepository<AgeGroup>
    {
        public AgeGroupRepository() { }

        public async Task<List<AgeGroup>> GetAllAgeGroup()
        {
            return await _context.AgeGroups.ToListAsync();
        }
        public async Task<AgeGroup> GetAgeGroupById(int id)
        {
            return await _context.AgeGroups.FirstOrDefaultAsync(i => i.GroupId == id);
        }
    }
}
