using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Assessment
{
    public class CreateAssessmentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssessmentType { get; set; }
        public int? AgeGroup { get; set; }
        public List<CreateQuestionDto> Questions { get; set; }
    }


}
