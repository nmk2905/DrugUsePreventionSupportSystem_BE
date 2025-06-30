using DTO.CourseQuestion.CourseOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CourseQuestion
{
	public class CourseQuestionDto
	{
		public int QuestionId { get; set; }

		public int? CourseId { get; set; }

		public string? QuestionText { get; set; }

		public int DisplayOrder { get; set; }

		public bool? IsRequired { get; set; }

		public virtual ICollection<CourseQuestionOptionDto> CourseQuestionOptions { get; set; } = new List<CourseQuestionOptionDto>();
	}
}
