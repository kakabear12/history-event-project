using BusinessObjectsLayer.Models;
using DTOs.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class PostMetaRequestModel
    {
        
        [Required(ErrorMessage = "PostId is a required field")]
        public int PostId { get; set; }
  
        [Required(ErrorMessage = "Key is a required field")]
        public string Keys { get; set; }

        [Required(ErrorMessage = "Contents is a required field")]
        public string Contents { get; set; }

        
    }
}
