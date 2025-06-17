using Repositories;
using Repositories.Enums;
using Repositories.Enums.Appointment;
using Repositories.Models;

namespace Services
{
    public interface IAppointmentService
    {
        Task<Appointment> BookAppointmentAsync(int userId, int availabilityId);
        Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId);
        Task<bool> CancelAppointmentAsync(int appointmentId, int userId);
        Task<bool> UpdateStatusAsync(int appointmentId, string status);
    }

    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly AvailabilityRepository _availabilityRepo;

        public AppointmentService()
        {
            _appointmentRepo = new AppointmentRepository();
            _availabilityRepo = new AvailabilityRepository();
        }

        public async Task<Appointment> BookAppointmentAsync(int userId, int availabilityId)
        {
            var slot = await _availabilityRepo.GetByIdAsync(availabilityId);
            if (slot == null || !slot.IsAvailable)
                throw new Exception("Slot không tồn tại hoặc đã được đặt.");

            var appointment = new Appointment
            {
                UserId = userId,
                ConsultantId = slot.ConsultantId,
                AvailabilityId = availabilityId,
                CreatedDate = DateTime.UtcNow,
                Status = AppointmentStatus.Pending
            };
            try
            {
                await _appointmentRepo.CreateAsync(appointment);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo appointment: " + ex.InnerException?.Message ?? ex.Message);
            }

            slot.IsAvailable = false;
            await _availabilityRepo.UpdateAsync(slot);

            return appointment;
        }


        public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId)
        {
            var all = await _appointmentRepo.GetAllAsync();
            return all.Where(a => a.UserId == userId || a.Consultant?.ConsultantNavigation?.UserId == userId).ToList();
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId, int userId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null || appointment.UserId != userId)
                return false;

            if (appointment.Status != AppointmentStatus.Pending)
                throw new Exception("Chỉ được hủy lịch ở trạng thái Pending.");

            var slot = await _availabilityRepo.GetByIdAsync(appointment.AvailabilityId ?? 0);
            if (slot != null)
            {
                slot.IsAvailable = true;
                await _availabilityRepo.UpdateAsync(slot);
            }

            await _appointmentRepo.RemoveAsync(appointment);
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int appointmentId, string status)
        {
            if (!new[] { AppointmentStatus.Pending, AppointmentStatus.Confirmed, AppointmentStatus.Completed, AppointmentStatus.Canceled }.Contains(status))
                throw new Exception("Trạng thái không hợp lệ.");

            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return false;

            appointment.Status = status;
            await _appointmentRepo.UpdateAsync(appointment);
            return true;
        }
    }
}