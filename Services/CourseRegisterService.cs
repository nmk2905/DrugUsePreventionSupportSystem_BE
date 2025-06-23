using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public interface ICourseRegisterService
	{
		Task<List<CourseRegister>> GetAllAsync();
	}

	public class CourseRegisterService : ICourseRegisterService
	{
		private readonly CourseRegisterRepository _repo;

		public CourseRegisterService(CourseRegisterRepository repo)
		{
			_repo = repo;
		}

		public async Task<List<CourseRegister>> GetAllAsync()
		{
			return await _repo.GetAllAsync();
		}
	}
}
