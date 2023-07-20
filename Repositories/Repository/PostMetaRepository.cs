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
    public class PostMetaRepository : Repository<PostMeta>
    {
        private readonly DbSet<PostMeta> _dbSet;
        private readonly HistoryEventDBContext _context;
        public PostMetaRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<PostMeta>();
        }
        public async Task<PostMeta> GetPostMetaById(int postId, int metaId)
        {
            return await _dbSet.Include(i => i.Images).SingleOrDefaultAsync(p => p.PostId == postId && p.Id == metaId);
        }


        public async Task<IEnumerable<PostMeta>> GetPostMetaByPostId(int postId)
        {
            return await _dbSet.Include(i => i.Images).Where(p => p.PostId == postId).ToListAsync();
        }

        public async Task<PostMeta> GetPostMetaWithImageById(int id)
        {
            return await _dbSet.Include(c => c.Images).SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
