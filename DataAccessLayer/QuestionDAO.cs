using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class QuestionDAO
    {
        private readonly HistoryEventDBContext context;
        public QuestionDAO(HistoryEventDBContext context)
        {
            this.context = context;
        }
     
    }
}
