using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        //Get all
        [HttpGet("Get-All-Approved")]
        [EnableQuery]
        public async Task<List<Blog>> GetAllApproved()
        {
            return await _blogService.GetAllApproved();
        }

        //List post cua user cu the da post
        [HttpGet("My-Blogs")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> GetMyBlogs()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var all = await _blogService.GetAllForAdmin();
            var mine = all.Where(b => b.AuthorId == userId).ToList();
            return Ok(mine);
        }



        //Get by id
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "1,3")]
        public async Task<Blog> GetById(int id)
        {
            return await _blogService.GetById(id);
        }

        [HttpPost("Post-blog")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Post([FromBody] PostBlogRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var blog = new Blog
            {
                Title = request.Title,
                Content = request.Content,
                PublishedDate = DateTime.UtcNow,
                AuthorId = userId,
                Status = "Pending"
            };

            var blogId = await _blogService.Create(blog);

            return Ok(blogId); 
        }

        public sealed record PostBlogRequest(string Title, string Content);


        //update
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Put(int id, [FromBody] PostBlogRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 1) Load blog từ DB
            var blog = await _blogService.GetById(id);
            if (blog == null)
                return NotFound();

            // 2) Kiểm tra bài này có phải của user không?
            if (blog.AuthorId != userId)
                return Forbid();

            // 3) Chỉ cho phép update khi còn Pending
            if (blog.Status != "Pending")
                return BadRequest("Cannot update an approved or rejected blog.");

            // 4) Chỉ update các trường được phép
            blog.Title = request.Title;
            blog.Content = request.Content;

            // 5) Gọi service update
            await _blogService.Update(blog);

            return Ok("Updated successfully");
        }


        //delete
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "3")]
        public async Task<bool> Delete(int id)
        {
            return await _blogService.Delete(id);
        }
    }
}
