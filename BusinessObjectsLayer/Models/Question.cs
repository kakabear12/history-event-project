using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class Question
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }
        public virtual Event Event { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; }
    }
}
