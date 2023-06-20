using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string PostContent { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
