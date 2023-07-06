using BusinessObjectsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class PostResponseModel
    {
        public int PostId { get; set; }
        public string AuthorName { get; set; }
        public int? ParentId { get; set; }
        public string MetaTitle { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public byte Published { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }


        public List<PostMetaResponseModel> PostMetas { get; set; }

        

        // Other properties as needed
    }

}
