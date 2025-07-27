using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AgeGroup
{
    public class AgeGroupSampleDto
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
    }
}
