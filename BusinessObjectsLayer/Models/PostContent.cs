using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectsLayer.Models
{
    public class PostContent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostContentId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string Document { get; set; }
        public virtual Post Post { get; set; }
    }
}
