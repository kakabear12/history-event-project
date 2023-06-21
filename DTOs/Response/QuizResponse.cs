using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class QuizResponse
    {
        public int QuizId { get; set; }
        public int Time { get; set; }
        public int NumberQuestion { get; set; }
    }
}
