using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserAssessment
{
    public class AssessmentResultResponse
    {
        public int TotalScore { get; set; }
        public string RiskLevel { get; set; }
        public string Recommendation { get; set; }
    }

}
