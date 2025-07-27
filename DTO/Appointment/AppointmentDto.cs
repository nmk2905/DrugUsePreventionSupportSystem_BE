using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Appointment
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public string ConsultantName { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string? MeetingLink { get; set; }
        public int? MaterialId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
