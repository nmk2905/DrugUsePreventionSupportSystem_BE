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

        public async Task<List<ConsultantsAvailability>> GetAvailableSlotsWithConsultant(int userId, DateOnly from, DateOnly to)
        {
            var consultantNumber = await _repo.GetConsultantNumberByUserIdAsync(userId);

            if (consultantNumber == null)
                throw new Exception($"Không tìm thấy consultant với userId = {userId}");

            return await _repo.GetAvailableSlotsWithConsultant(consultantNumber.Value, from, to);
        }



        public async Task<List<ConsultantsAvailability>> GetAvailableSlots(int userId, DateOnly from, DateOnly to)
        {
            var consultantNumber = await _repo.GetConsultantNumberByUserIdAsync(userId);

            if (consultantNumber == null)
                throw new Exception($"Không tìm thấy consultant với userId = {userId}");

            return await _repo.GetAvailableSlotsWithConsultant(consultantNumber.Value, from, to);
        }



        public async Task<ConsultantsAvailability> CreateSlot(int userId, DateOnly date, TimeOnly start, TimeOnly end)
        {
            // Lấy số hiệu (Number) của consultant từ userId
            var consultantNumber = await _repo.GetConsultantNumberByUserIdAsync(userId);

            if (consultantNumber == null)
                throw new Exception($"Không tìm thấy consultant với userId = {userId}");

            var slot = new ConsultantsAvailability
            {
                ConsultantId = consultantNumber.Value, // Đây mới là giá trị đúng
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
