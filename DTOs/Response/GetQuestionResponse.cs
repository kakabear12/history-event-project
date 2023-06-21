using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class GetQuestionResponse
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public virtual IEnumerable<GetAnswerToDoResponse> Answers { get; set; }
    }
}
