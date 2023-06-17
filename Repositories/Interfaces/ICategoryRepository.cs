using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task CreateCategory(Category category);
        Task DeleteCategory(int id);
        Task<Category> UpdateCategory(Category category);
    }
}
