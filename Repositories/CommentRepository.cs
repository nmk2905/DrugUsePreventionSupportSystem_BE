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
    public class CommentRepository : GenericRepository<Comment>
    {
        public CommentRepository() { }

        public async Task<List<Comment>> GetAll()
        {
            return await _context.Comments
                                 .Include(c => c.Blog)  
                                 .Include(c => c.User)  
                                 .ToListAsync();
        }


        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments
                                 .Include(c => c.Blog) 
                                 .Include(c => c.User)  
                                 .FirstOrDefaultAsync(c => c.CommentId == id);
        }

    }
}
