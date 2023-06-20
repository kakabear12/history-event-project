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
        public async Task DeleteCategory(int id)
        {
            try
            {

                var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
                if(category == null)
                {
                    throw new CustomException("Category not found");
                }
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<Category> UpdateCategory(Category category)
        {
            try
            {
                var categoryUpdate = await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
                if (category == null)
                {
                    throw new CustomException("Category not found");
                }
                categoryUpdate.CategoryName = category.CategoryName;
                await context.SaveChangesAsync();
                return categoryUpdate;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
