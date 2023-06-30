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
        public int AuthorId { get; set; }
        
        public int? ParentId { get; set; }
        [Required]
        public string MetaTitle { get; set; }

        [Required]
        public string Slug { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public byte Published { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public DateTime PublishedAt { get; set; }
        [Required]
        public string Content { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
       
        public virtual ICollection<PostComment> PostComments { get; set; }
       
        public virtual ICollection<PostMeta> PostMetas { get; set; }
       
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        
        public virtual ICollection<PostTag> PostTags { get; set; }

        [ForeignKey("ParentId")]
        public virtual Post ParentPost { get; set; }

        public virtual ICollection<Post> ChildPosts { get; set; }
    }
}
