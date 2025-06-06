using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAll();
        Task<Comment> GetById(int id);
        Task<int> Create(Comment comment);
        Task<int> Update(Comment comment);
        Task<bool> Delete(int id);
    }
    public class CommentService : ICommentService
    {
        private CommentRepository _repository;

        public CommentService()
        {
            _repository = new CommentRepository();
        }

        public async Task<int> Create(Comment comment)
        {
            return await _repository.CreateAsync(comment);
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

        public async Task<List<Comment>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public Task<Comment> GetById(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> Update(Comment comment)
        {
            return _repository.UpdateAsync(comment);
        }
    }
}
