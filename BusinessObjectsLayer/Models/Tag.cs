using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectsLayer.Models
{
    public class Tag
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string MetaTitle { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Contents { get; set; }
       
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
