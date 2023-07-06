using BusinessObjectsLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    

    public class CatesgoryRepository : Repository<Category>
    {
        private readonly DbSet<Category> _dbSet;
        private readonly HistoryEventDBContext _context;
        public CatesgoryRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Category>();
        }

        // Implement any additional methods specific to the EventsRepository here

        public async Task<Category> GetCategoryByName(string categoryName)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        }



    }
}
