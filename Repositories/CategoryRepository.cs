using BusinessObjectsLayer.Models;
using DataAccessLayer;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAO categoryDAO;
        public CategoryRepository(CategoryDAO categoryDAO)
        {
            this.categoryDAO = categoryDAO;
        }
        public async Task CreateCategory(Category category)
        {
            await categoryDAO.CreateCategory(category);
        }

        public async Task DeleteCategory(int id)
        {
            await categoryDAO.DeleteCategory(id);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await categoryDAO.GetCategoriesAsync();
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            return await categoryDAO.UpdateCategory(category);
        }
    }
}
