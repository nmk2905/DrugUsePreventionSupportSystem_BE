using DTO.CourseCategory;
using DTO.CourseQuestion;
using System.Text.Json.Serialization;

namespace DTO.Course
{
	public class CourseDto
	{
		public int CourseId { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public int Duration { get; set; }

		public DateTime? CreatedAt { get; set; }

		public string VideoUrl { get; set; } = string.Empty;

		public string DocumentContent { get; set; } = string.Empty;
		[JsonIgnore]
		public int? CategoryId { get; set; }
		public CourseCategoryDto Category { get; set; }
		public List<CourseQuestionDto> CourseQuestions { get; set; } = new List<CourseQuestionDto>();
	}
}
