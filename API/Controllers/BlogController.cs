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
        public async Task<List<BlogDTO>> GetAllApproved()
        {
            var blogs = await _blogService.GetAllApproved();
            var dtos = blogs.Select(b => new BlogDTO
            {
                BlogId = b.BlogId,
                Title = b.Title,
                Content = b.Content,
                AuthorId = b.AuthorId,
                PublishedDate = b.PublishedDate,
                Status = b.Status,
                AuthorFullName = b.Author?.FullName
            }).ToList();

            return dtos;
        }



        [HttpGet("My-Blogs")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> GetMyBlogs()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var all = await _blogService.GetAllForAdmin();

            var mine = all.Where(b => b.AuthorId == userId)
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

            return Ok(mine);
        }




        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<BlogDTO>> GetById(int id)
        {
            var blog = await _blogService.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }

            var dto = new BlogDTO
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Content = blog.Content,
                AuthorId = blog.AuthorId,
                PublishedDate = blog.PublishedDate,
                Status = blog.Status,
                AuthorFullName = blog.Author?.FullName
            };

            return Ok(dto);
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
