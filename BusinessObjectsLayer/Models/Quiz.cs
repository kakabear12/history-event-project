using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class Quiz
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizId { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public int NumberQuestion { get; set; }
        public virtual Event Event { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; }
        public virtual ICollection<QuizResult> QuizResults { get; set; }
    }
}
