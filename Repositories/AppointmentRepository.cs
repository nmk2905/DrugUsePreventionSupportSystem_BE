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

        public async Task<Appointment> GetAppointmentByUserId(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(i => i.UserId == id);
        }
    }
}
