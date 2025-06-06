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
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository() { }

        public async Task<User> GetUserAccount(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return _context.Users.ToList();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.UserId == id);
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            return await _context.Users.AnyAsync(e => e.Email == email);
        }
    }
}