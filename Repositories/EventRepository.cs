using BusinessObjectsLayer.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly HistoryEventDBContext _context;
        private readonly EventDAO eventDAO;
        public EventRepository(EventDAO eventDAO, HistoryEventDBContext context)
        {
            this.eventDAO = eventDAO;
            _context = context;
        }
        public async Task<Event> GetEventById(int id)
        {
            return await eventDAO.GetEventById(id);
        }

        public async Task<IEnumerable<Event>> SearchEventsByName(string keyword)
        {
            return await eventDAO.SearchEvents(keyword);
        }
    }
}
