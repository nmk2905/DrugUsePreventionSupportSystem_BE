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
    public class BlogRepository : GenericRepository<Blog>
    {
        public BlogRepository() { }

        public async Task<List<Blog>> GetAllForAdmin()
        {
            return await _context.Blogs.Include(b => b.Author).ToListAsync();
        }
        public async Task<List<Blog>> GetAllApproved()
        {
            return await _context.Blogs
                                 .Include(b => b.Author)
                                 .Where(b => b.Status == "Approved")
                                 .ToListAsync();
        }


        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs
                                 .Include(b => b.Author) 
                                 .FirstOrDefaultAsync(b => b.BlogId == id);
        }

    }
}
