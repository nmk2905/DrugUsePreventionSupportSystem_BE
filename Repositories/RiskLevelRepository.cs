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
    public class RiskLevelRepository :GenericRepository<RiskLevel>
    {
        public RiskLevelRepository() { }

        public async Task<List<RiskLevel>> GetAllRiskLevel()
        {
            return await _context.RiskLevels.ToListAsync();
        }
        public async Task<RiskLevel> GetRiskLevelById(int id)
        {
            return await _context.RiskLevels.FirstOrDefaultAsync(i => i.RiskId == id);
        }
    }
}
