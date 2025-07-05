using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Certificate
{
	public class CertificateDto
	{
		public int UserId { get; set; }

		public int CourseId { get; set; }

		public string Name { get; set; }

		public string CertificationCode { get; set; }

		public string CertificationUrl { get; set; }

		public DateOnly? AchievedDate { get; set; }
	}
}
