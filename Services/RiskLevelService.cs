using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IRiskLevelService
    {
        Task<List<RiskLevel>> GetAllRiskLevel();
        Task<RiskLevel> GetRiskLevelById(int id);
        Task<int> AddRiskLevelAsync(RiskLevel risk);
        Task<int> UpdateRiskLevelAsync(RiskLevel risk);
        Task<bool> DeleteRiskLevelAsync(int id);
    }
    public class RiskLevelService : IRiskLevelService
    {
        private readonly RiskLevelRepository _repository;
        public RiskLevelService()
        {
            _repository = new RiskLevelRepository();
        }
        public Task<int> AddRiskLevelAsync(RiskLevel risk)
        {
            return _repository.CreateAsync(risk);
        }

        public async Task<bool> DeleteRiskLevelAsync(int id)
        {
            var item = await _repository.GetRiskLevelById(id);
            if (item != null)
            {
                return await _repository.RemoveAsync(item);
            }
            return false;
        }

        public Task<List<RiskLevel>> GetAllRiskLevel()
        {
            return _repository.GetAllRiskLevel();
        }

        public Task<RiskLevel> GetRiskLevelById(int id)
        {
            return _repository.GetRiskLevelById(id);
        }

        public Task<int> UpdateRiskLevelAsync(RiskLevel risk)
        {
            return _repository.UpdateAsync(risk);
        }
    }
}
