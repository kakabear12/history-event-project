using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class PostCommentResponseModel
    {
        public int Id { get; set; }
   
        public int PostId { get; set; }

        public int? ParentId { get; set; }
    
        public string Title { get; set; }
   
        public byte Published { get; set; }
      
        public DateTime CreatedAt { get; set; }
  
        public DateTime PublishedAt { get; set; }
       
        public string Contents { get; set; }
    }
}
