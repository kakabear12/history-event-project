using BusinessObjectsLayer.Models;
using DTOs.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections;
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
            return await _dbSet.Include(i => i.Images).Where(p => p.AuthorId == authorId).ToListAsync();
        }

        public async Task<IEnumerable<Post>> FindByCondition(Expression<Func<Post, bool>> expression)
        {
            return await _dbSet.Include(i => i.Images).Where(expression).ToListAsync();
        }


        

        public async Task<Post> GetPostById(int id)
        {
            return await _dbSet.Include(p => p.PostMetas).ThenInclude(pm => pm.Images).ThenInclude(e =>e.Events)
                       .SingleOrDefaultAsync(p => p.PostId == id);
        }



        public async Task<IEnumerable<Post>> GetPostsByCategoryName(string categoryName)
        {
            return await _dbSet
                .Where(p => p.Categories.Any(c => c.CategoryName.ToLower() == categoryName.ToLower()))
                .Include(p => p.PostMetas).ThenInclude(pm => pm.Images)
                .Include(p => p.Categories)
                .ToListAsync();
        }


        public async Task<IEnumerable<Post>> SearchPostsByMetaTitle(string keyword)
        {
            return await _dbSet.Where(p => p.MetaTitle.Contains(keyword))
                                .Include(p => p.PostMetas).ThenInclude(pm => pm.Images).ToListAsync();    
        }


        public async Task<IEnumerable<Post>> GetAllAsyncs()
        {
            return await _dbSet.Include(p => p.PostMetas)
                .Include(p => p.Categories).Include(pm => pm.Images).Include(e => e.Events).ToListAsync();
        }


    }
}
