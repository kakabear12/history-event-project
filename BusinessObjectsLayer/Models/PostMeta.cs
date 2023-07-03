using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectsLayer.Models
{
    public class PostMeta
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Keys { get; set; }
        [Required]
        public string Contents { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
