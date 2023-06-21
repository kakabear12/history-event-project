using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class QuestionQuizResponse
    {
        public int QuestionTime { get; set; }

        public virtual GetQuestionResponse Question { get; set; }
    }
}
