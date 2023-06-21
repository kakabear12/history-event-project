using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class QuestionResponse
    {
        public int QuestionId { get; set; }
        public int EventId { get; set; }
        public string QuestionText { get; set; }
        public string DifficultyLevel { get; set; }
    }
}
