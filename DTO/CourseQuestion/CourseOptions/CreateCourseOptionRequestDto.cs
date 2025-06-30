using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CourseQuestion.CourseOptions
{
	public class CreateCourseOptionRequestDto
	{
		public string OptionText { get; set; }

		public int OptionValue { get; set; }

		public int DisplayOrder { get; set; }
	}
}
