using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class UpdateAnswerRequest
    {
        [Required(ErrorMessage = "Question id is a required field")]
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Answer id is a required field")]
        public int AnswerId { get; set; }
        [Required(ErrorMessage = "Text is a required field")]
        public string AnswerText { get; set; }
        [Required(ErrorMessage = "Is correct is a required field")]
        public bool IsCorrect { get; set; }
    }
}
