using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class UpdateQuestionRequest
    {
        [Required(ErrorMessage = "Question id is a required field")]
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Event id is a required field")]
        public int EventId { get; set; }
        [Required(ErrorMessage = "Question text is a required field")]
        public string QuestionText { get; set; }
        [EnumDataType(typeof(DifficultyLevel), ErrorMessage = "Invalid Difficulty Level. Difficulty Level are: Easy, Normal, Hard")]
        public string DifficultyLevel { get; set; }
    }
}
