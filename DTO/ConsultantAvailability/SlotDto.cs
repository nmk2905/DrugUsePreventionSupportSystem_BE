using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ConsultantAvailability
{
    public class SlotDto
    {
        public int AvailabilityId { get; set; }
        public DateOnly? SpecificDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string ConsultantName { get; set; }
    }
}
