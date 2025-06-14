using AutoMapper;
using DTO.User;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<UserDTO> Authenticate(string email, string password);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByEmail(string email);
        Task<UserDTO> GetUserById(int id);
        Task<bool> CheckEmailExist(string email);
        Task<UserDTO> RegisterAsync(RegisterUserDTO dto);
        Task<UserDTO> UpdateProfileAsync(UpdateProfileDTO dto);


    }

    public class UserService : IUserService
    {
        private readonly UserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper)
        {
            _repo = new UserRepository();
            _mapper = mapper;
        }

        public async Task<UserDTO> Authenticate(string email, string password)
        {
            var user = await _repo.GetUserAccount(email, password);
            return user == null ? null : MapToDto(user);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _repo.GetAllUsersAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user = await _repo.GetUserByEmail(email);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            return await _repo.CheckEmailExist(email);
        }

        private UserDTO MapToDto(User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth
            };
        }

        public async Task<UserDTO> RegisterAsync(RegisterUserDTO dto)
        {
            var exists = await _repo.CheckEmailExist(dto.Email);
            if (exists)
                throw new Exception("Email already exists");

            var user = _mapper.Map<User>(dto);
            user.CreatedDate = DateTime.UtcNow;

            await _repo.CreateAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateProfileAsync(UpdateProfileDTO dto)
        {
            var user = await _repo.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found");

            user.Email = dto.Email;
            user.FullName = dto.FullName;
            user.Address = dto.Address;
            user.DateOfBirth = dto.DateOfBirth;

            await _repo.UpdateAsync(user);

            return _mapper.Map<UserDTO>(user);
        }

    }

}
