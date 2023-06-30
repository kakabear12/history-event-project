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
    
    public class PostCommentRepository : Repository<PostComment>
    {
        private readonly DbSet<PostComment> _dbSet;
        private readonly HistoryEventDBContext _context;
        public PostCommentRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<PostComment>();
        }

        // Implement any additional methods specific to the EventsRepository here
    }
}
