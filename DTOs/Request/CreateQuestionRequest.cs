using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class CreateQuestionRequest
    {
        [Required(ErrorMessage = "Event Id is a required field")]
        public int EventId { get; set; }
        [Required(ErrorMessage = "QuestionText is a required field.")]
        public string QuestionText { get; set; }
        [Required(ErrorMessage = "Difficulty Level is required field.")]
        [EnumDataType(typeof(DifficultyLevel), ErrorMessage = "Invalid Difficulty Level. Difficulty Level are: Easy, Normal, Hard")]
        public string DifficultyLevel { get; set; }
        
    }
}
