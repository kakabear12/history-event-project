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
        public int Score { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; }
    }
}
