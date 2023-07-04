using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime? Birthday { get; set; }
        public int? TotalScore { get; set; }
        public int? TotalQuestion { get; set; }
        [Required]
        public Role Role { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }       
    }
}
