using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CourseQuestion.CourseOptions
{
	public class CourseQuestionOptionDto
	{
		public int OptionId { get; set; }

		public int? QuestionId { get; set; }

		public string OptionText { get; set; }

		public int OptionValue { get; set; }

		public int DisplayOrder { get; set; }
	}
}
