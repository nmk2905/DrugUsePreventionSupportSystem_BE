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
	public class CourseRegisterRepository : GenericRepository<CourseRegister>
	{
		public async Task<List<CourseRegister>> GetAllAsync()
		{
			return await _context.CourseRegisters.ToListAsync();
		}
	}
}
