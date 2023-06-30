using BusinessObjectsLayer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class PostRepository : Repository<Post>
    {
        private readonly DbSet<Post> _dbSet;
        private readonly HistoryEventDBContext _context;
        public PostRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Post>();
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorId(int authorId)
        {
            return await _dbSet.Where(p => p.AuthorId == authorId).ToListAsync();
        }

        public async Task<IEnumerable<Post>> FindByCondition(Expression<Func<Post, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }

    }
}
