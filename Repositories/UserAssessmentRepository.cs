using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.DBContext;
using Repositories.Models;

namespace Repositories
{
    public class UserAssessmentRepository : GenericRepository<UserAssessment>
    {
        public UserAssessmentRepository() { }

        public async Task<int> CalculateTotalScore(List<int> selectedOptionIds)
        {
            var validOptionCount = await _context.AssessmentOptions
                .CountAsync(o => selectedOptionIds.Contains(o.OptionId));

            return validOptionCount;
        }
    }
}
