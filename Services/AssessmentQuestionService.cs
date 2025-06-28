using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAssessmentQuestionService
    {
        Task<List<AssessmentQuestion>> GetAllAssessmentQuestion();
        Task<AssessmentQuestion> GetAssessmentQuestionById(int id);
        Task<int> AddQuestionAsync(AssessmentQuestion question);
        Task<int> UpdateQuestionAsync(AssessmentQuestion question);
        Task<bool> DeleteQuestionAsync(int id);
    }
    public class AssessmentQuestionService : IAssessmentQuestionService
    {
        private readonly AssessmentQuestionRepository _repository;
        public AssessmentQuestionService()
        {
            _repository = new AssessmentQuestionRepository();
        }

        public async Task<int> AddQuestionAsync(AssessmentQuestion question)
        {
            return await _repository.CreateAsync(question);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var item = await _repository.GetAssessmentQuestionById(id);
            if (item != null)
            {
                return await _repository.RemoveAsync(item);
            }

            return false;
        }

        public Task<AssessmentQuestion> GetAssessmentQuestionById(int id)
        {
            return _repository.GetAssessmentQuestionById(id);
        }

        public Task<List<AssessmentQuestion>> GetAllAssessmentQuestion()
        {
            return _repository.GetAllAssessmentQuestion();
        }

        public Task<int> UpdateQuestionAsync(AssessmentQuestion question)
        {
            return _repository.UpdateAsync(question);
        }
    }
}
