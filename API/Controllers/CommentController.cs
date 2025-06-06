using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories.Models;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        
        public CommentController(CommentService commentService)
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

        //create
        [HttpPost("Create")]
        public async Task<int> Post(Comment Comment)
        {
            return await _commentService.Create(Comment);
        }

        //update
        [HttpPut("Update/{id}")]
        public async Task<int> Put(Comment Comment)
        {
            return await _commentService.Update(Comment);
        }

        //delete
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _commentService.Delete(id);
        }
    }
}
