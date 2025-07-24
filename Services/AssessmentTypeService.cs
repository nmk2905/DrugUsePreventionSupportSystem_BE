using Azure;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAssessmentTypeService
    {
        Task<List<AssessmentType>> GetAllAssessmentType();
        Task<AssessmentType> GetAssessmentTypeById(int id);
        Task<int> AddAssessmentTypeAsync(AssessmentType type);
        Task<int> UpdateAssessmentTypeAsync(AssessmentType type);
        Task<bool> DeleteAssessmentTypeAsync(int id);
    }
    public class AssessmentTypeService : IAssessmentTypeService
    {
        private readonly AssessmentTypeRepository _repository;
        public AssessmentTypeService()
        {
            _repository = new AssessmentTypeRepository();
        }
        public Task<int> AddAssessmentTypeAsync(AssessmentType type)
        {
            return _repository.CreateAsync(type);
        }

        public async Task<bool> DeleteAssessmentTypeAsync(int id)
        {
            var item = await _repository.GetAssessmentTypeById(id);
            if (item != null)
            {
                return await _repository.RemoveAsync(item);
            }
            return false;
        }

        public Task<List<AssessmentType>> GetAllAssessmentType()
        {
            return _repository.GetAllAssessmentType();
        }

        public Task<AssessmentType> GetAssessmentTypeById(int id)
        {
            return _repository.GetAssessmentTypeById(id);
        }

        public Task<int> UpdateAssessmentTypeAsync(AssessmentType type)
        {
            return _repository.UpdateAsync(type);
        }
    }
}
