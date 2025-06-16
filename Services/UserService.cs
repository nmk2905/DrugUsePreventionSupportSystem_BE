using Repositories;
using Repositories.Models;

namespace Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task<bool> CheckEmailExist(string email);
        Task<User> RegisterAsync(User user);
        Task<User> UpdateProfileAsync(User user);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _repo;

        public UserService()
        {
            _repo = new UserRepository();
        }

        public async Task<User> Authenticate(string email, string password)
        {
            return await _repo.GetUserAccount(email, password);
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

        public async Task<bool> CheckEmailExist(string email)
        {
            return await _repo.CheckEmailExist(email);
        }

        public async Task<User> RegisterAsync(User user)
        {
            var exists = await _repo.CheckEmailExist(user.Email);
            if (exists)
                throw new Exception("Email already exists");

            user.CreatedDate = DateTime.UtcNow;
            await _repo.CreateAsync(user);
            return user;
        }

        public async Task<User> UpdateProfileAsync(User input)
        {
            var user = await _repo.GetByIdAsync(input.UserId);
            if (user == null)
                throw new Exception("User not found");

            user.Email = input.Email;
            user.FullName = input.FullName;
            user.Address = input.Address;
            user.Password = input.Password;
            user.DateOfBirth = input.DateOfBirth;

            await _repo.UpdateAsync(user);
            return user;
        }
    }
}
