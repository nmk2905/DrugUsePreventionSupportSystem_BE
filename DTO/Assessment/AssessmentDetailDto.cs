using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Assessment
{
    public class AssessmentDetailDto
    {
        public int AssessmentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssessmentType { get; set; }
        public string AgeGroup { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }

        public List<QuestionDto> Questions { get; set; }
    }
}
