using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task<bool> CheckEmailExist(string email);
    }
}
