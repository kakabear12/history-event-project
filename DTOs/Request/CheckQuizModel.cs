using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class CheckQuizModel
    {
        [Required(ErrorMessage = "Question id is a required field")]
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Answer id is a required field")]
        public int AnswerId { get; set; }
    }
}
