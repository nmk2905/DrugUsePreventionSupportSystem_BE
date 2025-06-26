using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.CourseQuestion
{
	public class CreateQuestionRequestDto
	{
		public string QuestionText { get; set; }
		public int DisplayOrder { get; set; }
		public bool? IsRequired { get; set; }
	}
}
