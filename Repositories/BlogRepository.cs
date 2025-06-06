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

        public async Task<List<Blog>> GetAll()
        {
            var Blogs = await _context.Blogs.ToListAsync();  
            return Blogs;
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            var Blogs = await _context.Blogs.FirstOrDefaultAsync(c => c.BlogId == id);
            return Blogs;
        }
    }
}
