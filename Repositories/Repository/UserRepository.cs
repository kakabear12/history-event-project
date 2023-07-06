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
    public class UsersRepository : Repository<User>
    {
        private readonly DbSet<User> _dbSet;
        private readonly HistoryEventDBContext _context;

        public UsersRepository(HistoryEventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbSet.FindAsync(userId);
        }
    }

}
