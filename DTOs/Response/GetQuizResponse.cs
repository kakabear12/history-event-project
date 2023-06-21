using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class GetQuizResponse
    {
        public int QuizId { get; set; }
        public int Time { get; set; }
        public int NumberQuestion { get; set; }
        public IEnumerable<QuestionQuizResponse> QuestionQuizzes { get; set; }
    }
}
