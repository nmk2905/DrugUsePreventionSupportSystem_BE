using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories.Models;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public UserController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }


        //login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.Authenticate(request.Email, request.Password);

            if (user == null)
                return Unauthorized();

            var token = GenerateJSONWebToken(user);

            return Ok(token);
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                new Claim[]
                {
            new(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
            new(ClaimTypes.Email, userInfo.Email),
            new(ClaimTypes.Role, userInfo.RoleId?.ToString() ?? "0")
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //[HttpGet("GetAllUser")]
        //[Authorize(Roles = "1")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _userService.GetAllUsersAsync();
        //    return Ok(users);
        //}

        //lấy data người dùng bằng email
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var user = await _userService.GetUserByEmail(email);
            if (user == null)
                return NotFound();

            var userDto = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Address = user.Address,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                CreatedDate = user.CreatedDate,
                RoleName = user.Role?.RoleName 
            };

            return Ok(userDto);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            var userDto = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Address = user.Address,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                CreatedDate = user.CreatedDate,
                RoleName = user.Role?.RoleName
            };

            return Ok(userDto);
        }


        //kiểm tra mail có tồn tại
        [HttpGet("CheckEmailExist")]
        public async Task<IActionResult> CheckEmailExist([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var exists = await _userService.CheckEmailExist(email);
            return Ok(new { exists });
        }

        //đăng kí, thành công sẽ tạo role user
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var newUser = new User
                {
                    FullName = request.FullName,
                    Address = request.Address,
                    Email = request.Email,
                    DateOfBirth = request.DateOfBirth,
                    Password = request.Password,
                    RoleId = 3, //customer
                    CreatedDate = DateTime.UtcNow
                };

                var result = await _userService.RegisterAsync(newUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //update profile
        [HttpPut("Update-Profile")]
        [Authorize(Roles ="1,2,3,4")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var user = await _userService.GetUserById(userId);
                if (user == null) return NotFound();

                user.FullName = request.FullName;
                user.Email = request.Email;
                user.Address = request.Address;
                user.DateOfBirth = request.DateOfBirth;

                var result = await _userService.UpdateProfileAsync(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //resetpassword
        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.NewPassword))
                return BadRequest("Email and new password are required.");

            var success = await _userService.ResetPasswordAsync(request.Email, request.NewPassword);
            if (!success)
                return NotFound("User with provided email does not exist.");

            return Ok(new { message = "Password has been reset successfully." });
        }


        [HttpGet("MyProfile")]
        public async Task<IActionResult> MyProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var user = await _userService.GetUserById(userId);
                if (user == null)
                    return NotFound("User not found.");

                var dto = new UserDTO
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Address = user.Address,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    CreatedDate = user.CreatedDate,
                    RoleName = user.Role?.RoleName
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        public class UserDTO
        {
            public int UserId { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public DateOnly DateOfBirth { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string RoleName { get; set; }
        }


        public sealed record LoginRequest(string Email, string Password);

        public sealed record UpdateProfileRequest(string Email, string FullName, string Address, DateOnly DateOfBirth);

        public sealed record ForgotPasswordRequest(string Email, string NewPassword);

        public sealed record RegisterRequest(
    string FullName,
    string Address,
    string Email,
    DateOnly DateOfBirth,
    string Password
);


    }

}
