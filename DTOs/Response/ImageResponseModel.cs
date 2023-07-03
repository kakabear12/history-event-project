using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Response
{
    public class ImageResponseModel
    {
        public long Id { get; set; }
      
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string Directory { get; set; }
     
        public float Size { get; set; }     
        public string Type { get; set; }
        public string Url { get; set; }
        public string Medium { get; set; }
        public string Small { get; set; }
        public string Thumb { get; set; }
        public string Caption { get; set; }
        public string AltText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
