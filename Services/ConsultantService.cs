using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IConsultantService
    {
        Task<List<Consultant>> GetAllConsultant();
        Task<Consultant> GetConsultantById(int id);
        Task<Consultant> UpdateProfileAsync(Consultant consultant);
    }
    public class ConsultantService : IConsultantService
    {
        private readonly ConsultantRepository _repository;

        public ConsultantService()
        {
            _repository = new ConsultantRepository();
        }
        public async Task<List<Consultant>> GetAllConsultant()
        {
            return await _repository.GetAllConsultant();
        }

        public async Task<Consultant> GetConsultantById(int id)
        {
           return await _repository.GetConsultantById(id);
        }

        public async Task<Consultant> UpdateProfileAsync(Consultant input)
        {
            var cons = await _repository.GetByIdAsync(input.ConsultantId);
            if (cons == null)
                throw new Exception("Consultant not found");

            cons.Specification = input.Specification;
            cons.Qualifications = input.Qualifications;
            cons.ExperienceYears = input.ExperienceYears;

            await _repository.UpdateAsync(cons);
            return cons;
        }
    }
}
