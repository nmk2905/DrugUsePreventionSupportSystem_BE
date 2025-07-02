using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.DBContext;
using Repositories.Models;

namespace Repositories
{
    public class UserAssessmentRepository : GenericRepository<UserAssessment>
    {
        public UserAssessmentRepository() { }
    }
}
