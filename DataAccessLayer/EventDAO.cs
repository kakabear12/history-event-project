using BusinessObjectsLayer.Models;
using DTOs.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EventDAO
    {
        private readonly HistoryEventDBContext context;
        public EventDAO(HistoryEventDBContext context)
        {
            this.context = context;
        }
        public async Task<Event> GetEventById(int id)
        {
            try {
                var ev = await context.Events.SingleOrDefaultAsync(e => e.EventId == id);
                if(ev == null)
                {
                    throw new CustomException("Event not found");
                }
                return ev;
            }catch(CustomException ex)
            {
                throw new CustomException(ex.Message);
            }
        }
    }
}
