using DTO.Blog;
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

        //lấy a toàn bộ blog được duyệt (status = approved)
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


        //show all blog của user
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



        //lấy data 1 blog
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


        // đăng blog
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

            var blog = await _blogService.GetById(id);
            if (blog == null)
                return NotFound();

            if (blog.AuthorId != userId)
                return Forbid();

            if (blog.Status != "Pending")
                return BadRequest("Cannot update an approved or rejected blog.");

            blog.Title = request.Title;
            blog.Content = request.Content;

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
