using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserAnswer
{
	public class UserAnswerDto
	{
		public int? CourseId { get; set; }
		public int? UserId { get; set; }
		public int? QuestionId { get; set; }
		public int? OptionId { get; set; }
		public int? TotalPoint { get; set; }
		public DateTime? AnswerAt { get; set; }
	}
}
