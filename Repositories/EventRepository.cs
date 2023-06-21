using BusinessObjectsLayer.Models;
using DataAccessLayer;
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
        private readonly EventDAO eventDAO;
        public EventRepository(EventDAO eventDAO)
        {
            this.eventDAO = eventDAO;
        }
        public async Task<Event> GetEventById(int id)
        {
            return await eventDAO.GetEventById(id);
        }
    }
}
