using Repositories;
using Repositories.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _repo;

        public UserService() => _repo = new UserRepository();

        public async Task<User> Authenticate(string email, string password)
        {
            return await _repo.GetUserAccount(email, password);
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            return await _repo.CheckEmailExist(email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repo.GetAllUsersAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _repo.GetUserByEmail(email);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
    }
}
