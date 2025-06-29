using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAgeGroupService
    {
        Task<List<AgeGroup>> GetAllAgeGroup();
        Task<AgeGroup> GetAgeGroupById(int id);
        Task<int> AddAgeGroupAsync(AgeGroup age);
        Task<int> UpdateAgeGroupAsync(AgeGroup age);
        Task<bool> DeleteAgeGroupAsync(int id);
    }
    public class AgeGroupService : IAgeGroupService
    {
        private readonly AgeGroupRepository _repository;
        public AgeGroupService()
        {
            _repository = new AgeGroupRepository();
        }
        public Task<int> AddAgeGroupAsync(AgeGroup age)
        {
            return _repository.CreateAsync(age);
        }

        public async Task<bool> DeleteAgeGroupAsync(int id)
        {
            var item = await _repository.GetAgeGroupById(id);
            if (item != null)
            {
                return await _repository.RemoveAsync(item);
            }
            return false;
        }

        public Task<AgeGroup> GetAgeGroupById(int id)
        {
            return _repository.GetAgeGroupById(id);
        }

        public Task<List<AgeGroup>> GetAllAgeGroup()
        {
            return _repository.GetAllAgeGroup();
        }

        public Task<int> UpdateAgeGroupAsync(AgeGroup age)
        {
            return _repository.UpdateAsync(age);
        }
    }
}
