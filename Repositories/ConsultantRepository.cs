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
    public class ConsultantRepository : GenericRepository<Consultant>
    {
        private readonly Drug_use_prevention_systemContext _context;

        public ConsultantRepository(Drug_use_prevention_systemContext context) : base(context)
        {
            _context = context;
        }

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
