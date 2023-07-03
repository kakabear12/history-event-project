using CommandLine.Text;
using NHibernate.Criterion;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class ImageRequestModel
    {
        [Required(ErrorMessage = "Name is a required field")]

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public string Directory { get; set; }
       
        public float? Size { get; set; }
        [Required(ErrorMessage = "Type is a required field")]    
        public string Type { get; set; }
 
        public string Medium { get; set; }
        public string Small { get; set; }
        public string Thumb { get; set; }
        public string Caption { get; set; }
        public string AltText { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
