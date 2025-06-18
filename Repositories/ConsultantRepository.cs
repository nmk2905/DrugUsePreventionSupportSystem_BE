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
    public class ConsultantRepository : GenericRepository<Consultant>
    {
        public ConsultantRepository() { }

        public async Task<List<Consultant>> GetAllConsultant()
        {
            return await _context.Consultants
                                 .Include(c => c.ConsultantNavigation) 
                                 .ToListAsync();
        }

        public async Task<Consultant> GetConsultantById(int id)
        {
            return await _context.Consultants
                                 .Include(c => c.ConsultantNavigation) 
                                 .FirstOrDefaultAsync(i => i.ConsultantId == id);
        }

    }
}
