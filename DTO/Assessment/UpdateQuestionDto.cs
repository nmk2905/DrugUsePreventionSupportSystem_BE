using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Assessment
{
    public class UpdateQuestionDto
    {
        public int? QuestionId { get; set; } 
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public List<UpdateOptionDto> Options { get; set; }
    }
}
