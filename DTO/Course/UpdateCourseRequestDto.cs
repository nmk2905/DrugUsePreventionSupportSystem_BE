﻿namespace DTO.Course
{
	public class UpdateCourseRequestDto
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int Duration { get; set; }
		public string VideoUrl { get; set; } = string.Empty;
		public string DocumentContent { get; set; } = string.Empty;
		public int? CategoryId { get; set; }
	}
}
