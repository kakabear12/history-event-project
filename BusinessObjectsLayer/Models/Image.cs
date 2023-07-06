using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectsLayer.Models
{
    public class Image
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string Directory { get; set; }
        [Required]
        public float Size { get; set; }
        [Required]
        public string Type { get; set; }
        public string Url { get; set; }
        public string Medium { get; set; }
        public string Small { get; set; }
        public string Thumb { get; set; }
        public string Caption { get; set; }
        public string AltText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<PostMeta> PostMetas { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }




    }
}
