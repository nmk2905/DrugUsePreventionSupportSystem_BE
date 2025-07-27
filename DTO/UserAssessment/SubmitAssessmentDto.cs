using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserAssessment
{
    public class SubmitAssessmentDto
    {
        public int UserId { get; set; }
        public int AssessmentId { get; set; }
        public List<int> SelectedOptionIds { get; set; }
    }
}
