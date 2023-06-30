using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class TagRequestModel
    {
        [Required(ErrorMessage = "Title is a required field")]
        public string Title { get; set; }
        [Required(ErrorMessage = "MetaTitle is a required field")]
        public string MetaTitle { get; set; }
        [Required(ErrorMessage = "Slug is a required field")]
        public string Slug { get; set; }
        [Required(ErrorMessage = "Contents is a required field")]
        public string Contents { get; set; }
    }
}
