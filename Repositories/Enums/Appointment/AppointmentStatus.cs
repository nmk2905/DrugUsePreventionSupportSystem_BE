using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enums.Appointment
{
    public static class AppointmentStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string Completed = "Completed";
        public const string Canceled = "Canceled";
    }
}
