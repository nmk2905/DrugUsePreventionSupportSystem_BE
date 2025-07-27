using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AssessmentOption
{
    public class OptionSampleDto
    {
        public int OptionId { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string OptionText { get; set; }
        public int? OptionValue { get; set; }
    }
}
