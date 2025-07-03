using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserAnswer
{
	public class SubmitAnswerRequestDto
	{
		public int QuestionId { get; set; }
		public int OptionId { get; set; }
	}
}
