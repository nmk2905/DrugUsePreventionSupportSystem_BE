using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AssessmentQuestion
{
    public class UpdateQuestionDto
    {
        public int? AssessmentId { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }
    }
}
