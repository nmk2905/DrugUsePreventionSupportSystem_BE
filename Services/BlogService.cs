using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAll();
        Task<Blog> GetById(int id);
        Task<int> Create(Blog blog);
        Task<int> Update(Blog blog);
        Task<bool> Delete(int id);
    }
    public class BlogService : IBlogService
    {
        private BlogRepository _repository;

        public BlogService()
        {
            _repository = new BlogRepository();
        }
        public async Task<int> Create(Blog blog)
        {
            return await _repository.CreateAsync(blog);
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item != null)
            {
                return await _repository.RemoveAsync(item);
            }

            return false;
        }

        public async Task<List<Blog>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public Task<Blog> GetById(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> Update(Blog blog)
        {
            return _repository.UpdateAsync(blog);
        }
    }
}
