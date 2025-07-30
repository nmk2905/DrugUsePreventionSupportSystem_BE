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
    public class AppointmentRepository : GenericRepository<Appointment>
    {
        public AppointmentRepository() { }

        public async Task<List<Appointment>> GetAppointmentsByUserId(int userId)
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Consultant)
                    .ThenInclude(c => c.ConsultantNavigation)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Consultant)
                    .ThenInclude(c => c.ConsultantNavigation)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<List<Appointment>> GetAllForAdmin()
        {
            return await _context.Appointments.Include(a => a.User)
                .Include(a => a.Consultant).ThenInclude(c => c.ConsultantNavigation).ToListAsync();
        }

    }
}
