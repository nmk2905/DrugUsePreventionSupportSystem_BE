using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Assessment
{
    public class UpdateFullAssessmentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssessmentType { get; set; }
        public int? AgeGroup { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public List<UpdateQuestionDto> Questions { get; set; }
    }
}
