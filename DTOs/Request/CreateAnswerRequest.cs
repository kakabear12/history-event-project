using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class CreateAnswerRequest
    {
        [Required(ErrorMessage = "Question Id is a required field")]
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Answer text is a required field")]
        public string AnswerText { get; set; }
        [Required(ErrorMessage = "Is correct is a required field")]
        public bool IsCorrect { get; set; }
    }
}
