using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public interface IAvailabilityService
    {
        Task<List<ConsultantsAvailability>> GetAvailableSlots(int consultantId, DateOnly from, DateOnly to);
        Task<ConsultantsAvailability> CreateSlot(int consultantId, DateOnly date, TimeOnly start, TimeOnly end);
        Task<ConsultantsAvailability> GetByIdWithConsultantAsync(int id);
        Task<List<ConsultantsAvailability>> GetAvailableSlotsWithConsultant(int consultantId, DateOnly from, DateOnly to);
    }

    public class AvailabilityService : IAvailabilityService
    {
        private readonly AvailabilityRepository _repo;

        public AvailabilityService()
        {
            _repo = new AvailabilityRepository();
        }

        public async Task<ConsultantsAvailability> GetByIdWithConsultantAsync(int id)
        {
            return await _repo.GetByIdWithConsultantAsync(id);
        }

        public async Task<List<ConsultantsAvailability>> GetAvailableSlotsWithConsultant(int consultantId, DateOnly from, DateOnly to)
        {
            return await _repo.GetAvailableSlotsWithConsultant(consultantId, from, to);
        }


        public async Task<List<ConsultantsAvailability>> GetAvailableSlots(int consultantId, DateOnly from, DateOnly to)
        {
            return await _repo.GetAvailableSlotsWithConsultant(consultantId, from, to);
        }


        public async Task<ConsultantsAvailability> CreateSlot(int consultantId, DateOnly date, TimeOnly start, TimeOnly end)
        {
            var slot = new ConsultantsAvailability
            {
                ConsultantId = consultantId,
                SpecificDate = date,
                StartTime = start,
                EndTime = end,
                IsAvailable = true
            };

            await _repo.CreateAsync(slot);
            return slot;
        }
    }
}
