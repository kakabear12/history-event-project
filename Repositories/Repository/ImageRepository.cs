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
    
    public class ImageRepository : Repository<Image>
    {
        private readonly DbSet<Image> _dbSet;
        private readonly HistoryEventDBContext _context;
        public ImageRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Image>();
        }

        // Implement any additional methods specific to the EventsRepository here
        



    }
}
