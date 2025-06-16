using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories.Models;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        //Get all
        [HttpGet("GetAll")]
        [EnableQuery]
        public async Task<IEnumerable<Comment>> Get()
        {
            return await _commentService.GetAll();
        }

        //Get by id
        [HttpGet("GetById/{id}")]
        public async Task<Comment> GetById(int id)
        {
            return await _commentService.GetById(id);
        }


        public sealed record PostCommentRequest(int BlogId, string Content);


        //create
        [HttpPost("Create")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Post([FromBody] PostCommentRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var comment = new Comment
            {
                BlogId = request.BlogId,
                Content = request.Content,
                PostDate = DateTime.UtcNow,
                UserId = userId
            };

            var id = await _commentService.Create(comment);
            return Ok(new { CommentId = id });
        }


        public sealed record UpdateCommentRequest(string Content);


        //update
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCommentRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Lấy comment từ DB
            var comment = await _commentService.GetById(id);
            if (comment == null)
                return NotFound();

            // Kiểm tra quyền sở hữu
            if (comment.UserId != userId)
                return Forbid();

            // Cập nhật Content
            comment.Content = request.Content;

            await _commentService.Update(comment);

            return Ok("Updated successfully");
        }

        //delete
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var comment = await _commentService.GetById(id);
            if (comment == null)
                return NotFound();

            if (comment.UserId != userId)
                return Forbid();

            await _commentService.Delete(id);

            return Ok("Deleted successfully");
        }

        public sealed record MyCommentResponse(
    int CommentId,
    string Content,
    DateTime? PostDate,
    int BlogId,
    string BlogTitle,
    int? BlogAuthorId
);

        [HttpGet("My-Comment")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> GetMyComments()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var allComments = await _commentService.GetAll();

            var myComments = allComments
                .Where(c => c.UserId == userId)
                .Select(c => new MyCommentResponse(
                    c.CommentId,
                    c.Content,
                    c.PostDate,
                    c.BlogId ?? 0,
                    c.Blog?.Title ?? "",
                    c.Blog?.AuthorId
                ))
                .ToList();

            return Ok(myComments);
        }

        public sealed record CommentByBlogResponse(
    int CommentId,
    string Content,
    DateTime? PostDate,
    int? UserId,
    string UserName
);

        [HttpGet("All-Comment-By-Blog/{blogId}")]
        public async Task<IActionResult> GetCommentsByBlog(int blogId)
        {
            var allComments = await _commentService.GetAll();

            var comments = allComments
                .Where(c => c.BlogId == blogId)
                .Select(c => new CommentByBlogResponse(
                    c.CommentId,
                    c.Content,
                    c.PostDate,
                    c.UserId,
                    c.User?.FullName ?? ""
                ))
                .ToList();

            return Ok(comments);
        }



    }
}
