using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories.Models;
using Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        //Get all
        [HttpGet("GetAll")]
        [EnableQuery]
        public async Task<IEnumerable<Blog>> Get()
        {
            return await _blogService.GetAll();
        }

        //Get by id
        [HttpGet("GetById/{id}")]
        public async Task<Blog> GetById(int id)
        {
            return await _blogService.GetById(id);
        }

        //create
        [HttpPost("Create")]
        public async Task<int> Post(Blog blog)
        {
            return await _blogService.Create(blog);
        }

        //update
        [HttpPut("Update/{id}")]
        public async Task<int> Put(Blog blog)
        {
            return await _blogService.Update(blog);
        }

        //delete
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _blogService.Delete(id);
        }
    }
}
