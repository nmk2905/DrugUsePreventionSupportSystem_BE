using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAssessmentService
    {
        Task<List<Assessment>> GetAllAssessment();
        Task<Assessment> GetAssessmentById(int id);
        Task<int> UpdateAssessmentAsync(Assessment asm);
        Task<bool> DeleteAssessmentAsync(int id);
        //
        Task<int> CreateFullAssessmentAsync(Assessment assessment);
    }
    public class AssessmentService : IAssessmentService
    {
        private readonly AssessmentRepository _repo;
        public AssessmentService(AssessmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> DeleteAssessmentAsync(int id)
        {
            var item = await _repo.GetAssessmentById(id);
            if (item != null)
            {
                return await _repo.RemoveAsync(item);
            }
            return false;
        }

        public Task<List<Assessment>> GetAllAssessment()
        {
            return _repo.GetAllAssessment();
        }

        public Task<Assessment> GetAssessmentById(int id)
        {
            return _repo.GetAssessmentById(id);
        }

        public Task<int> UpdateAssessmentAsync(Assessment asm)
        {
            return _repo.UpdateAsync(asm);
        }

        //
        public async Task<int> CreateFullAssessmentAsync(Assessment assessment)
        {
            return await _repo.AddAssessmentAsync(assessment);
        }
    }
}
