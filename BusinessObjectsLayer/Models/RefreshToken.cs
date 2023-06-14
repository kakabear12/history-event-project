using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectsLayer.Models
{
    public class RefreshToken
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [StringLength(1000)]
        [Required]
        public string TokenHash { get; set; }
        [StringLength(50)]
        public string TokenSalt { get; set; }
        [Required]
        public DateTime Ts { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        public virtual User User { get; set; }
    }
}
