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

        [HttpGet("GetAllUser")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("All-blogs-for-admin")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetPendingBlogs()
        {
            var blogs = await _blogService.GetAllForAdmin();
            var pending = blogs.Where(b => b.Status == "Pending").ToList();
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
    }

}
