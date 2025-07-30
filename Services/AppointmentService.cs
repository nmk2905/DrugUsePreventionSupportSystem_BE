using Repositories;
using Repositories.Enums;
using Repositories.Enums.Appointment;
using Repositories.Models;

namespace Services
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllForAdmin();
        Task<Appointment> BookAppointmentAsync(int userId, int availabilityId);
        Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId);
        Task<bool> CancelAppointmentAsync(int appointmentId, int userId);
        Task<bool> UpdateStatusAsync(int appointmentId, string status, string? meetingLink = null);
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

        public async Task<List<Appointment>> GetAllForAdmin()
        {
            return await _appointmentRepo.GetAllForAdmin();
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
                Status = AppointmentStatus.Pending,
                MeetingLink = null
            };

            await _appointmentRepo.CreateAsync(appointment);

            // Cập nhật lại slot
            slot.IsAvailable = false;
            await _availabilityRepo.UpdateAsync(slot);

            var created = await _appointmentRepo.GetByIdAsync(appointment.AppointmentId);

            return created;
        }



        public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId)
        {
            return await _appointmentRepo.GetAppointmentsByUserId(userId);
        }


        public async Task<bool> CancelAppointmentAsync(int appointmentId, int userId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);

            if (appointment == null || appointment.UserId != userId)
                return false;

            if (appointment.Status != AppointmentStatus.Pending)
                throw new Exception("Chỉ được hủy lịch ở trạng thái Pending.");

            appointment.Status = AppointmentStatus.Canceled;
            await _appointmentRepo.UpdateAsync(appointment);

            // Mở lại slot nếu cần
            if (appointment.AvailabilityId.HasValue)
            {
                var slot = await _availabilityRepo.GetByIdAsync(appointment.AvailabilityId.Value);
                if (slot != null)
                {
                    slot.IsAvailable = true;
                    await _availabilityRepo.UpdateAsync(slot);
                }
            }

            return true;
        }


        public async Task<bool> UpdateStatusAsync(int appointmentId, string status, string? meetingLink = null)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return false;

            if (!new[] { AppointmentStatus.Pending, AppointmentStatus.Confirmed, AppointmentStatus.Completed, AppointmentStatus.Canceled }
                .Contains(status))
                throw new Exception("Trạng thái không hợp lệ.");

            appointment.Status = status;

            // Gán link nếu admin duyệt
            if (status == AppointmentStatus.Confirmed)
            {
                if (string.IsNullOrWhiteSpace(meetingLink))
                    throw new Exception("Meeting link không được bỏ trống khi xác nhận lịch.");

                appointment.MeetingLink = meetingLink;
            }

            await _appointmentRepo.UpdateAsync(appointment);
            return true;
        }

    }
}