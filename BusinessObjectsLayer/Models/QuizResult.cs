using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class QuizResult
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizResultId { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual User User { get; set; }
    }
}
