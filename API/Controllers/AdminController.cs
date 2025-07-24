using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class AdminController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public AdminController(IBlogService blogService, IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        //lấy tất cả người dùng các role
        [HttpGet("GetAllUser")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var userDTOs = users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Address = u.Address,
                Email = u.Email,
                DateOfBirth = u.DateOfBirth,
                Password = u.Password,
                CreatedDate = u.CreatedDate,
                RoleName = u.Role?.RoleName
            }).ToList();

            return Ok(userDTOs);
        }

        //lấy các blog chờ được duyệt (các blog ở status pending")
        [HttpGet("All-blogs-for-admin")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetPendingBlogs()
        {
            var blogs = await _blogService.GetAllForAdmin();
            var pending = blogs.Where(b => b.Status == "Pending")
                               .Select(b => new BlogDTO
                               {
                                   BlogId = b.BlogId,
                                   Title = b.Title,
                                   Content = b.Content,
                                   AuthorId = b.AuthorId,
                                   PublishedDate = b.PublishedDate,
                                   Status = b.Status,
                                   AuthorFullName = b.Author?.FullName
                               })
                               .ToList();

            return Ok(pending);
        }


        // Admin duyệt blog
        [HttpPut("Approve/{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Approve(int id)
        {
            var blog = await _blogService.GetById(id);
            if (blog == null) return NotFound();

            blog.Status = "Approved";
            await _blogService.Update(blog);

            return Ok("Approved");
        }

        // Admin từ chối blog
        [HttpPut("Reject/{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Reject(int id)
        {
            var blog = await _blogService.GetById(id);
            if (blog == null) return NotFound();

            blog.Status = "Rejected";
            await _blogService.Update(blog);

            return Ok("Rejected");
        }

        //[HttpPut("UpdateRole")]
        //public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDTO request)
        //{
        //    try
        //    {
        //        var user = await _userService.GetUserById(request.UserId);
        //        if (user == null)
        //            return NotFound(new { message = "User not found." });

        //        // Cập nhật RoleId
        //        user.RoleId = request.RoleId;

        //        var result = await _userService.UpdateProfileAsync(user); // hoặc tạo phương thức riêng nếu cần

        //        return Ok(new { message = "Role updated successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}


        //public class UpdateRoleDTO
        //{
        //    public int UserId { get; set; }
        //    public int RoleId { get; set; }
        //    public string RoleName { get; set; }
        //}


        public class UserDTO
        {
            public int UserId { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public DateOnly DateOfBirth { get; set; }
            public string Password { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string RoleName { get; set; }
        }

        public class BlogDTO
        {
            public int BlogId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public int? AuthorId { get; set; }
            public DateTime? PublishedDate { get; set; }
            public string Status { get; set; }
            public string AuthorFullName { get; set; }
        }


    }

}
