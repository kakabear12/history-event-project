using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class CreatePostRequestModel
    {
      
        

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "MetaTitle is a required field")]

        public string MetaTitle { get; set; }

        [Required(ErrorMessage = "Slug is a required field")]

        public string Slug { get; set; }

        [Required(ErrorMessage = "Summary is a required field")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Published is a required field")]
        public byte Published { get; set; }

        [Required(ErrorMessage = "CreatedAt is a required field")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "UpdatedAt is a required field")]
        public DateTime UpdatedAt { get; set; }

        [Required(ErrorMessage = "PublishedAt is a required field")]
        public DateTime PublishedAt { get; set; }

        [Required(ErrorMessage = "Content is a required field")]
        public string Content { get; set; }

        public List<string> CategoryNames { get; set; }

        public List<string> EventNames { get; set; }

        // Other properties as needed
    }

}
