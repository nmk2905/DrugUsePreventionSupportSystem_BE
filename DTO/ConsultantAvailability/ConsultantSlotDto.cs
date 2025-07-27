using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ConsultantAvailability
{
    public class ConsultantSlotDto
    {
        public int AvailabilityId { get; set; }
        public int? ConsultantId { get; set; }
        public DateOnly? SpecificDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public string Consultant { get; set; }
    }
}
