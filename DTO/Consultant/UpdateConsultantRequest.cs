using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Consultant
{
    public class UpdateConsultantRequest
    {
        public string Specification { get; set; }
        public string Qualifications { get; set; }
        public int ExperienceYears { get; set; }
    }
}
