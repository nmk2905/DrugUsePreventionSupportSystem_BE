using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Assessment
{
    public class CreateQuestionDto
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public List<CreateOptionDto> Options { get; set; }
    }
}
