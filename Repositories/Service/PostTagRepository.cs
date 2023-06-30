using BusinessObjectsLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Service
{
    
    public class PostTagRepository : Repository<PostTag>
    {
        private readonly DbSet<PostTag> _dbSet;
        private readonly HistoryEventDBContext _context;
        public PostTagRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<PostTag>();
        }
        public async Task<IEnumerable<PostTag>> FindByCondition(Expression<Func<PostTag, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }


        public async Task<PostTag> FindSingleByCondition(Expression<Func<PostTag, bool>> expression)
        {
            return await _dbSet.SingleOrDefaultAsync(expression);
        }

        // Implement any additional methods specific to the EventsRepository here
    }
}
