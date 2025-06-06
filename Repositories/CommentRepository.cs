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
            var Comments = await _context.Comments.Include(c => c.Blog).ToListAsync();
            return Comments;
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            var Comments = await _context.Comments.Include(c => c.Blog).FirstOrDefaultAsync(c => c.CommentId == id);
            return Comments;
        }
    }
}
