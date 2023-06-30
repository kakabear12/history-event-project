using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class EventRequestModel
    {
        [Required(ErrorMessage = "EventName is a required field")]
        public string EventName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Description is a required field")]
        public string Description { get; set; }
    }
}
