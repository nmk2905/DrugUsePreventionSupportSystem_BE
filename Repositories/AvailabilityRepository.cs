using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class AvailabilityRepository : GenericRepository<ConsultantsAvailability>
    {
        public AvailabilityRepository() { }

        public async Task<List<ConsultantsAvailability>> GetAvailableSlotsWithConsultant(int consultantId, DateOnly from, DateOnly to)
        {
            return await _context.ConsultantsAvailabilities
                .Include(ca => ca.Consultant)
                    .ThenInclude(c => c.ConsultantNavigation)
                .Where(ca => ca.ConsultantId == consultantId
                          && ca.IsAvailable
                          && ca.SpecificDate >= from
                          && ca.SpecificDate <= to)
                .ToListAsync();
        }

        public async Task<ConsultantsAvailability> GetByIdWithConsultantAsync(int id)
        {
            return await _context.ConsultantsAvailabilities
                .Include(ca => ca.Consultant)
                    .ThenInclude(c => c.ConsultantNavigation)
                .FirstOrDefaultAsync(ca => ca.AvailabilityId == id);
        }


    }
}
