using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class PostCommentRequestModel
    {
        [Required(ErrorMessage = "PostId is a required field")]

        public int PostId { get; set; }

        public int? ParentId { get; set; }
        [Required(ErrorMessage = "Title is a required field")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Published is a required field")]

        public byte Published { get; set; }
        [Required(ErrorMessage = "CreatedAt is a required field")]

        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "PublishedAt is a required field")]

        public DateTime PublishedAt { get; set; }
        [Required(ErrorMessage = "Contents is a required field")]

        public string Contents { get; set; }
    }
}
