using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Assessment
{
    public class UpdateOptionDto
    {
        public int? OptionId { get; set; } 
        public string OptionText { get; set; }
        public int? OptionValue { get; set; }
    }
}
