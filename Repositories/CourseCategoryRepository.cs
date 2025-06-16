using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
	public class CourseCategoryRepository : GenericRepository<CourseCategory>
	{
		public CourseCategoryRepository()
		{

		}

		public async Task<List<CourseCategory>> GetAllAsync()
		{
			return await _context.CourseCategories.ToListAsync();
		}

		public async Task<CourseCategory?> GetByIdAsync(int id)
		{
			return await _context.CourseCategories.FirstOrDefaultAsync(c => c.CategoryId == id);
		}

		public async Task<CourseCategory> CreateAsync(CourseCategory category)
		{
			await _context.CourseCategories.AddAsync(category);
			await _context.SaveChangesAsync();
			return category;
		}

		public async Task<CourseCategory?> UpdateAsync(int id, CourseCategory category)
		{
			var categoryModel = await _context.CourseCategories.FirstOrDefaultAsync(c => c.CategoryId == id);

			if (category == null) 
				return null;

			categoryModel.Name = category.Name;
			categoryModel.Description = category.Description;
			categoryModel.Age = category.Age;

			await _context.SaveChangesAsync();
			return category;
		}


	}
}
