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
    
    public class TagRepository : Repository<Tag>
    {
        private readonly DbSet<Tag> _dbSet;
        private readonly HistoryEventDBContext _context;
        public TagRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Tag>();
        }

        

        // Implement any additional methods specific to the EventsRepository here
    }
}
