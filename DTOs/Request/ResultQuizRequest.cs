using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class ResultQuizRequest
    {
        [Required(ErrorMessage = "Quiz id is a required field")]
        public int QuizId { get; set; }
        public IEnumerable<CheckQuizModel> Quizzes { get; set; }
    }
}
