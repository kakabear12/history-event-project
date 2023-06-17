using BusinessObjectsLayer.Models;
using DTOs.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CategoryDAO
    {
        private readonly HistoryEventDBContext context;
        public CategoryDAO(HistoryEventDBContext context)
        {
            this.context = context;
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                return await context.Categories.ToListAsync();
            }catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task CreateCategory(Category category)
        {
            try
            {
                var newCategory = await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
