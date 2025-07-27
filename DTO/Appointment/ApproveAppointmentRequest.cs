using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Appointment
{
    public class ApproveAppointmentRequest
    {
        public int AppointmentId { get; set; }
        public string MeetingLink { get; set; }
    }
}
