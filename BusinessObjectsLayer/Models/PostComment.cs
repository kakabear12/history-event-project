using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectsLayer.Models
{
    public class PostComment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
        
        public int? ParentId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public byte Published { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime PublishedAt { get; set;}
        [Required]
        public string Contents { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        [ForeignKey("ParentId")]
        public virtual PostComment ParentPost { get; set; }

        public virtual ICollection<PostComment> ChildPostComments { get; set; }
    }
}
