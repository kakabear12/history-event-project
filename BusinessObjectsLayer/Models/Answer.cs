using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class Answer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        [Required]
        public string AnswerText { get; set; }
        [Required]
        public bool IsCorrect { get; set; }

        public virtual Question Question { get; set; }
    }
}
