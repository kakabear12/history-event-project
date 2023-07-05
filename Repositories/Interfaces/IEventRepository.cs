using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<Event> GetEventById(int id);
        Task<List<Event>> SearchEventsByName(string keyword);
    }
}
