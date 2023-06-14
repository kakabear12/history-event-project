using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class QuestionQuiz
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionQuizId { get; set; }
        [Required]
        public int QuestionTime { get; set; }

        public virtual Question Question { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
