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

        //show all comment
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var comments = await _commentService.GetAll();

            var dtoList = comments.Select(c => new CommentDTO
            {
                CommentId = c.CommentId,
                Content = c.Content,
                PostDate = c.PostDate,
                BlogId = c.BlogId,
                BlogTitle = c.Blog?.Title ?? "",
                UserId = c.UserId,
                UserFullName = c.User?.FullName ?? ""
            }).ToList();

            return Ok(dtoList);
        }

        //show comment theo id
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentService.GetById(id);

            if (comment == null)
                return NotFound();

            var dto = new CommentDTO
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                PostDate = comment.PostDate,
                BlogId = comment.BlogId,
                BlogTitle = comment.Blog?.Title ?? "",
                UserId = comment.UserId,
                UserFullName = comment.User?.FullName ?? ""
            };

            return Ok(dto);
        }



        public sealed record PostCommentRequest(int BlogId, string Content);


        //tạo comment
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

            var comment = await _commentService.GetById(id);
            if (comment == null)
                return NotFound();

            if (comment.UserId != userId)
                return Forbid();

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

        //show all comment của user
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

        
        //show all cmt của 1 blog chỉ định
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

        public class CommentDTO
        {
            public int CommentId { get; set; }
            public string Content { get; set; }
            public DateTime? PostDate { get; set; }
            public int? BlogId { get; set; }
            public string BlogTitle { get; set; }
            public int? UserId { get; set; }
            public string UserFullName { get; set; }
        }


        public sealed record MyCommentResponse(
    int CommentId,
    string Content,
    DateTime? PostDate,
    int BlogId,
    string BlogTitle,
    int? BlogAuthorId
);

        public sealed record CommentByBlogResponse(
    int CommentId,
    string Content,
    DateTime? PostDate,
    int? UserId,
    string UserName
);



    }
}
