using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CourseQuestion
{
	public class UpdateCourseQuestionRequestDto
	{
		public string QuestionText { get; set; }
		public int DisplayOrder { get; set; }
	}
}
