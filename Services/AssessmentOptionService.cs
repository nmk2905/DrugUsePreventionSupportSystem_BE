using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAssessmentOptionService
    {
        Task<List<AssessmentOption>> GetAllAssessmentOption();
        Task<AssessmentOption> GetAssessmentOptionById(int id);
        Task<int> AddOptionAsync(AssessmentOption option);
        Task<int> UpdateOptionAsync(AssessmentOption option);
        Task<bool> DeleteOptionAsync(int id);
    }

    public class AssessmentOptionService : IAssessmentOptionService
    {
        private readonly AssessmentOptionRepository _repository;

        public AssessmentOptionService()
        {
            _repository = new AssessmentOptionRepository();
        }

        public Task<int> AddOptionAsync(AssessmentOption option)
        {
            return _repository.CreateAsync(option);
        }

        public async Task<bool> DeleteOptionAsync(int id)
        {
            var item = await _repository.GetAssessmentOptionById(id);
            if (item != null)
            {
                return await _repository.RemoveAsync(item);
            }
            return false;
        }

        public Task<List<AssessmentOption>> GetAllAssessmentOption()
        {
            return _repository.GetAllAssessmentOption();
        }

        public Task<AssessmentOption> GetAssessmentOptionById(int id)
        {
            return _repository.GetAssessmentOptionById(id);
        }

        public Task<int> UpdateOptionAsync(AssessmentOption option)
        {
            return _repository.UpdateAsync(option);
        }
    }
}
