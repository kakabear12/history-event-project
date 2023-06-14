using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BusinessObjectsLayer.Models
{
    public partial class Document
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }
        public string Link { get; set; }

        public virtual Post Post { get; set; }
    }
}
