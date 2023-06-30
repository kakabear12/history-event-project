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
    public class EventsRepository : Repository<Event>
    {
        private readonly DbSet<Event> _dbSet;
        private readonly HistoryEventDBContext _context;
        public EventsRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Event>();
        }

        // Implement any additional methods specific to the EventsRepository here
    }
}
